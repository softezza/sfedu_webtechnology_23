import org.apache.spark.sql.SparkSession
import org.apache.spark.sql.{DataFrame, SparkSession}

import org.apache.spark.sql.functions.explode
import org.apache.spark.sql.expressions.UserDefinedFunction
import org.apache.spark.sql.functions.udf
import org.apache.spark.sql.functions.{when, _}
import java.net.{URL, URLDecoder}
import scala.util.Try

import org.apache.spark.sql.functions.{col}


//import spark.implicits._
import org.apache.spark.sql.functions.{substring, length}


object data_mart {
  //Внутри класса
  def main(args: Array[String]): Unit = {
    // Внутри метода



    val spark = SparkSession.builder()
      .config("spark.cassandra.connection.host", "10.0.0.31")
      .config("spark.cassandra.connection.port", "9042")
      .config("spark.master", "local")
      .getOrCreate()



    val clients: DataFrame = spark.read
      .format("org.apache.spark.sql.cassandra")
      .options(Map("table" -> "clients", "keyspace" -> "labdata"))
      .load()


    val visits: DataFrame = spark.read
      .format("org.elasticsearch.spark.sql")
      .options(Map("es.read.metadata" -> "true",
        "es.nodes.wan.only" -> "true",
        "es.port" -> "9200",
        "es.nodes" -> "10.0.0.31",
        "es.net.ssl" -> "false"))
      .load("visits")


    val logs: DataFrame = spark.read
      .json("hdfs:///labs/laba03/weblogs.json")


    val cats: DataFrame = spark.read
      .format("jdbc")
      .option("url", "jdbc:postgresql://10.0.0.31:5432/labdata")
      .option("dbtable", "domain_cats")
      .option("user", "artem_voloshenko")
      .option("password", "bDQu5t0u")
      .option("driver", "org.postgresql.Driver")
      .load()



    clients.createOrReplaceTempView("clients")



    val client_with_age_cat = spark.sql("""

        select
             *
            ,case
                when age between 18 and 24 then "18-24"
                when age between 25 and 34 then "25-34"
                when age between 35 and 44 then "35-44"
                when age between 45 and 54 then "45-54"
                when age >= 55 then ">=55" end as age_cat
        from clients

    """)

    client_with_age_cat.createOrReplaceTempView("client_with_age_cat")

    val visits_null_filter = spark.sql("select * from visits where uid is not null")




    val uid_time_url = logs.select(col("uid") , explode(col("visits")))

    val uid_url = uid_time_url.select(col("uid"), col("col.url"))


    def decodeUrlAndGetDomain: UserDefinedFunction = udf((url: String) => {
      Try {
        new URL(URLDecoder.decode(url, "UTF-8")).getHost
      }.getOrElse("")
    })


    val uid_domain_with_www = uid_url.withColumn("url" , decodeUrlAndGetDomain(col("url")).name("domain"))


    val uid_domain = uid_domain_with_www.withColumn("url",
      when(col("url").like("www.%"), expr("substring(url, 5, length(url))"))
        .otherwise(col("url")))



    cats.createOrReplaceTempView("cats")
    uid_domain.createOrReplaceTempView("uid_domain")


    val uid_domain_cat = spark.sql("""

          select

               u.uid
              ,c.domain
              ,concat("web_" , category) as category


          from
              uid_domain    as u
          left join
              cats          as c
          on
              u.url = c.domain

     """)


    uid_domain_cat.createOrReplaceTempView("uid_domain_cat")
    visits_null_filter.createOrReplaceTempView("visits_null_filter")
    client_with_age_cat.createOrReplaceTempView("client_with_age_cat")



    val shop_tire_nahui = visits_null_filter.withColumn("category" ,  regexp_replace(col("category"), "-", "_"))

    val shop_lower = shop_tire_nahui.withColumn("category" ,  lower(col("category")))

    val shop_prefix = shop_lower.withColumn("category" , concat(lit("shop_") , col("category")))

    val shop_pivot_prefix = shop_prefix.groupBy(col("uid")).pivot("category").count().na.fill(0)

    val web_pivot = uid_domain_cat.groupBy(col("uid")).pivot("category").count().na.fill(0)


    web_pivot.createOrReplaceTempView("web_pivot")
    shop_pivot_prefix.createOrReplaceTempView("shop_pivot_prefix")


    val res = spark.sql(
      """

          select

               client_with_age_cat.uid
              ,client_with_age_cat.gender

              ,client_with_age_cat.age_cat

              ,shop_cameras, shop_clothing, shop_computers, shop_cosmetics, shop_entertainment_equipment, shop_everyday_jewelry, shop_house_repairs_paint_tools, shop_household_appliances, shop_household_furniture, shop_kitchen_appliances, shop_kitchen_utensils, shop_luggage, shop_mobile_phones, shop_shoes, shop_sports_equipment, shop_toys

              ,web_arts_and_entertainment, web_autos_and_vehicles, web_beauty_and_fitness, web_books_and_literature, web_business_and_industry, web_career_and_education, web_computer_and_electronics, web_finance, web_food_and_drink, web_gambling, web_games, web_health, web_home_and_garden, web_internet_and_telecom, web_law_and_government, web_news_and_media, web_pets_and_animals, web_recreation_and_hobbies, web_reference, web_science, web_shopping, web_sports, web_travel


          from
              client_with_age_cat
          left join
              web_pivot
          on
              web_pivot.uid = client_with_age_cat.uid
          left join
              shop_pivot_prefix
          on
              shop_pivot_prefix.uid = client_with_age_cat.uid

      """)



    res.show()



  }
}