ThisBuild / version := "0.1.0-SNAPSHOT"

ThisBuild / scalaVersion := "2.11.12"

lazy val root = (project in file("."))
  .settings(
    name := "lab03"
  )


libraryDependencies += "org.apache.spark" % "spark-core_2.11" % "2.4.7"
libraryDependencies += "org.apache.spark" % "spark-sql_2.11" % "2.4.7"
libraryDependencies += "com.datastax.spark" %% "spark-cassandra-connector" % "2.4.3"
libraryDependencies += "org.elasticsearch" %% "elasticsearch-spark-20" % "6.8.2"
libraryDependencies += "org.postgresql" % "postgresql" % "42.3.3"




