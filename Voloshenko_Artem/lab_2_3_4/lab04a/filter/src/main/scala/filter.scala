import org.apache.spark.sql.{DataFrame, SparkSession}
import org.apache.spark.sql.types.{DateType, LongType, TimestampType}
import org.apache.spark.sql.functions.{col}
import org.apache.spark.sql.functions._
import org.apache.spark.sql.functions.{col,from_json}
import org.apache.spark.sql.types._


object filter {
  //Внутри класса
  def main(args: Array[String]): Unit = {


//    val spark: SparkSession = SparkSession.builder()
//      .config("spark.filter.topic_name", "lab04_input_data")
//      .config("spark.filter.output_dir_prefix", "visits")
//      .config("spark.filter.offset", "earliest")
//      .getOrCreate()
//
//


    val spark: SparkSession = SparkSession.builder()
      .getOrCreate()

    val spark2 = spark
    import spark2.implicits._


    //spark.conf.set("kafka.bootstrap.servers", "spark-master-1.newprolab.com:6667")
    spark.conf.set("spark.sql.session.timeZone", "UTC")

    var topicName: String = spark.conf.get("spark.filter.topic_name")
    var outputDir: String = spark.conf.get("spark.filter.output_dir_prefix")
    var offset: String = spark.conf.get("spark.filter.offset")


    val kafkaData = spark.read.format("kafka")
      .option("kafka.bootstrap.servers",  "spark-master-1.newprolab.com:6667")
      .option("subscribe", topicName)
      .option("startingOffsets",
        if (offset.contains("earliest")) offset else {
          "{\"" + topicName + "\":{\"0\":" + offset + "}}"
        }).load




    val jsonStr = kafkaData.select( col("value").cast("string") ).as[String]


    val schema = new StructType()
      .add("event_type", StringType, true)
      .add("category", StringType, true)
      .add("item_id", StringType, true)
      .add("item_price", IntegerType, true)
      .add("uid", StringType, true)
      .add("timestamp", StringType, true)



    val jsonDF = jsonStr.withColumn("value", from_json(col("value") , schema)).select("value.*")

    val buy = jsonDF.withColumn("date" , to_date(from_unixtime(col("timestamp") / 1000))).filter(col("event_type") === "buy").withColumn("date", regexp_replace(col("date"), "-", ""))

    val view = jsonDF.withColumn("date" , to_date(from_unixtime(col("timestamp") / 1000))).filter(col("event_type") === "view").withColumn("date", regexp_replace(col("date"), "-", ""))

    val buyDF = buy.select(col("*") , col("date").as("p_date") )

    val viewDF = view.select(col("*") , col("date").as("p_date") )

    buyDF.write.mode("overwrite").partitionBy("p_date").json(outputDir + "/buy/")

    viewDF.write.mode("overwrite").partitionBy("p_date").json(outputDir + "/view/")

  }
}