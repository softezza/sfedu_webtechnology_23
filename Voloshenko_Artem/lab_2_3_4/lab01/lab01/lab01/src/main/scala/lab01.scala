import scala.io.BufferedSource
import scala.collection.mutable.ListBuffer
import scala.io.Source.fromFile
import scala.collection.immutable.ListMap
import java.io._
import org.json4s._
import org.json4s.JsonDSL._
import org.json4s.jackson.JsonMethods._
import scala.collection.immutable.ListMap

object lab01 {
  //Внутри класса
  def main(args: Array[String]): Unit = {
    // Внутри метода

    object udata {


      val source: BufferedSource = fromFile("./ml-100k/u.data")
      val lines: Seq[Array[String]] = source.getLines.toList.map(string => string.split("\t"))
      source.close()
    }

    val df = udata.lines.filter(_(1) == f)

    df.groupBy(identity).mapValues(_.size)

    var marks = new ListBuffer[String]()

    for (i <- df) {
      marks.append(i(2))
    }

    marks.groupBy(identity).mapValues(_.size)

    val map_id =  ListMap(marks.groupBy(identity).mapValues(_.size).toSeq.sortBy(_._1):_*)

    var value = new Array[Int](5)
    value(0) = map_id("1")
    value(1) = map_id("2")
    value(2) = map_id("3")
    value(3) = map_id("4")
    value(4) = map_id("5")


    val json_map_id = Map("hist_film" -> value)
    val json = Map("hist_film" -> (14, 40, 156, 228, 148))

    val js = scala.util.parsing.json.JSONObject(json)

    import com.fasterxml.jackson.module.scala.DefaultScalaModule

    val mapper = new ObjectMapper()
    mapper.registerModule(DefaultScalaModule)
    mapper.writeValueAsString(Map("a" -> 1))


    val json = ("hist_film" -> List(14, 40, 156, 228, 148))

    compact(render(json))

    import java.io._

    //...
    //внутри класса/объекта:
    object to_json {


      //создаем файл
      val file: File = new File("lab01.json")
      //создаем writer для записи файла
      val writer: BufferedWriter = new BufferedWriter(new FileWriter(file))

      //пишем строку в файл

      writer.write(compact(render(json)))
      //освобождаем ресурсы writer
      writer.close()
    }

    to_json.writer


    var marks_all = new ListBuffer[String]()


    for (i <- udata.lines) {
      marks_all.append(i(2))
    }


    marks_all.groupBy(identity).mapValues(_.size)

    val map__all_id =  ListMap(marks_all.groupBy(identity).mapValues(_.size).toSeq.sortBy(_._1):_*)

    var value_all = new Array[Int](5)
    value_all(0) = map__all_id("1")
    value_all(1) = map__all_id("2")
    value_all(2) = map__all_id("3")
    value_all(3) = map__all_id("4")
    value_all(4) = map__all_id("5")


    val json_film = (("hist_film" -> List(7, 20, 78, 114, 74) ) , ("hist_all" -> List(6110, 11370, 27145, 34174, 21201)))


    val json_film = ("hist_film" -> List(7, 20, 78, 114, 74))
    val json_all = ("hist_all" -> List(6110, 11370, 27145, 34174, 21201))
    val fucking_json = Map(json_film, json_all)




    //...
    //внутри класса/объекта:
    object to_json {


      //создаем файл
      val file: File = new File("lab01.json")
      //создаем writer для записи файла
      val writer: BufferedWriter = new BufferedWriter(new FileWriter(file))

      //пишем строку в файл

      writer.write(compact(render(fucking_json)))
      //освобождаем ресурсы writer
      writer.close()
    }

    to_json.writer





  }