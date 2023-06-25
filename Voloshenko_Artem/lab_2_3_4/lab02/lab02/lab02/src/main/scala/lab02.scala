import org.apache.spark.sql.types._
import org.apache.spark.sql.expressions.UserDefinedFunction
import org.apache.spark.sql.functions.udf
import org.apache.spark.sql.functions.{col}
import java.net.{URL, URLDecoder}
import scala.util.Try
import org.apache.spark.sql.functions.{when, _}
import org.apache.spark.sql.functions.{substring, length}
import org.apache.spark.sql.functions.format_number


object lab02 {
  //Внутри класса
  def main(args: Array[String]): Unit = {
    // Внутри метода


    object df {


      val autousersPath = "/labs/laba02/autousers.json"
      val logs_path = "/labs/laba02/logs"

      val usersSchema =
        StructType(
          List(
            StructField("autousers", ArrayType(StringType, containsNull = false))
          )
        )

      val users = spark.read.schema(usersSchema).json(autousersPath)

      val logsSchema =
        StructType(
          List(
            StructField("uid", StringType),
            StructField("ts", StringType),
            StructField("url", StringType)
          )
        )

      val domains = spark.read.schema(logsSchema).option("delimiter", "\t").csv(logs_path)

    }


    object decode {


      def decodeUrlAndGetDomain: UserDefinedFunction = udf((url: String) => {
        Try {
          new URL(URLDecoder.decode(url, "UTF-8")).getHost
        }.getOrElse("")
      })

      //   transformedLogs.select(  col("uid"), decodeUrlAndGetDomain(  col("url")).alias("domain")   )

    }


    val decode_domains = df.domains.select(col("uid"), decode.decodeUrlAndGetDomain(col("url")).alias("domain"))

    val auto_casc = df.users.withColumn("autousers", concat_ws(" ", $"autousers"))


    val auto = df.users.select(explode($"autousers")).distinct()


    val a = df.users.select(explode($"autousers"))


    df.users.select(size($"autousers").as("no_of_friends")).show

    ddm.createOrReplaceTempView("DDM")
    auto.createOrReplaceTempView("AUTO")

    val join_result = spark.sql("select * from DDM left join AUTO on DDM.uid = AUTO.col ")



    join_result.createOrReplaceTempView("join_result")

    val df = spark.sql("select domain as url , case when col is null then 0 else 1 end  as Auto_flag from join_result where domain <> '' ")


    val temp_df_1 = spark.sql(
      """
                SELECT *

                       ,sum(Auto_flag) over (partition by DF.url )  as group_sum_up
    --                     ,count(Auto_flag) over (partition by DF.url )  as group_count_down_1
    --                   ,sum(Auto_flag) as all_sum_down_2
                FROM DF

    """)

    temp_df_1.createOrReplaceTempView("temp_df_1")
    val temp_df_2 = spark.sql(
      """
                SELECT *

    --                   ,sum(Auto_flag) over (partition by DF.url )  as group_sum_up
                         ,count(Auto_flag) over (partition by url )  as group_count_down_1
    --                   ,sum(Auto_flag) as all_sum_down_2
                FROM temp_df_1

    """)


    temp_df_2.createOrReplaceTempView("temp_df_2")

    //spark.sql("select count(*) from temp_df_2 where Auto_flag = 1 ").show()


    val temp_df_3 = spark.sql(
      """
                SELECT *

    --                   ,sum(Auto_flag) over (partition by DF.url )  as group_sum_up
    --                     ,count(Auto_flag) over (partition by url )  as group_count_down_1
                       , 313523 as all_sum_down_2
                FROM temp_df_2

                """)


    temp_df_3 .createOrReplaceTempView("temp_df_3 ")


    val temp_df_4 = spark.sql(
      """
                SELECT *

                       , 6571023 as all_count_down_2
                FROM temp_df_3

      """)



    temp_df_4.createOrReplaceTempView("temp_df_4")


    val result_df = spark.sql(
      """
                SELECT url
                        , group_sum_up / all_count_down_2  * group_sum_up / all_count_down_2 / group_count_down_1 / all_count_down_2 * all_sum_down_2  / all_count_down_2 as relevance

                        , cast( (group_sum_up  * group_sum_up) /   (group_count_down_1 * all_sum_down_2)  as decimal(36,16)) as dec_rel
                        , (group_sum_up  * group_sum_up) /   (group_count_down_1 * all_sum_down_2)  as rel
                FROM temp_df_4

                """)
    result_df.orderBy(desc("relevance")).show(20, 80)


    result_df.createOrReplaceTempView("result_df")

    val distinct_result_df = spark.sql("select distinct * from result_df")

    distinct_result_df.createOrReplaceTempView("distinct_result_df")

    distinct_result_df.withColumn("dec_rel", format_number($"dec_rel", 15)).filter(distinct_result_df("url") === "avto-vykup.kiev.ua").show(20,80)

    spark.sql(
      """

                select
                         url
                        ,cast(dec_rel as string) as relevance
                from distinct_result_df
                order by dec_rel desc , url asc
                limit 200

    """).coalesce(1).write.mode("overwrite").option("delimiter", "\t").csv(result_path)


    import sys.process._
    """hdfs dfs -get lab02_result/part-* laba02_domains.txt""".!!


  }