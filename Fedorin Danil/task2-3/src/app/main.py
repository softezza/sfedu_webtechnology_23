from fastapi import FastAPI
from datetime import date
from pydantic import BaseModel
from json import JSONEncoder
import json
import uvicorn
import mysql.connector as mc

def getConnector():
	conn= mc.connect(host='db',user='root',password='654321',db='testingsystem_db')
	c = conn.cursor() 
	return c,conn
	
def commitConnector(c, conn):
	conn.commit()
	c.close()
	conn.close()


print("Connecting to db")
c, conn = getConnector()

c.execute('DROP TABLE IF EXISTS students')
c.execute('CREATE TABLE students \
    (                   \
      id    INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY, \
      login  VARCHAR(30) NOT NULL UNIQUE,  \
      password  VARCHAR(30) NOT NULL UNIQUE,  \
      assessment  VARCHAR(30) NOT NULL \
    )'
)
commitConnector(c, conn)
print("Connected to db")

app = FastAPI()
    
class Student(BaseModel):
    login: str
    password: str
    assessment: int
    

@app.get("/")
def index():
	return {"Hello, world!"}

@app.post("/sign_up/")
def signUpStudent(credentials: Student):
	login = credentials.login
	password = credentials.password
	assessment = credentials.assessment
	c, conn = getConnector()
	c.execute(f"SELECT  * FROM students WHERE login = '{login}' OR password = '{password}'")
	if len(c.fetchall()) > 0:
		commitConnector(c, conn)
		return {"registered" : False, "info":"students already created"}
	
	query = f"INSERT INTO students (login,password,assessment) VALUES('{login}','{password}','{assessment}');"
	c.execute(query)
	commitConnector(c, conn)
	return {"registered" : True}
	
@app.delete("/remove/")
def removeStudent(login: str, password: str):
	c, conn = getConnector()
	c.execute(f"SELECT  * FROM students WHERE login = '{login}' AND password = '{password}'")
	if len(c.fetchall()) == 0:
		commitConnector(c, conn)
		return {"info":"student not found"}
	
	query = f"DELETE FROM students WHERE login = '{login}' AND password = '{password}';"
	c.execute(query)
	commitConnector(c, conn)
	return {"unregistered" : True}
	
@app.get("/get_assessment/")
def getAssessment(login: str):
	c, conn = getConnector()
	c.execute(f"SELECT assessment FROM students WHERE login = '{login}'")
	assessment = c.fetchall()
	if len(assessment) == 0:
		commitConnector(c, conn)
		return {"info":"student not found"}
	return {"assessment" : assessment[0][0]}
	
@app.post("/update_assessment/")
def updateAssessment(credentials: Student):
	login = credentials.login
	password = credentials.password
	assessment = credentials.assessment
	c, conn = getConnector()
	c.execute(f"SELECT  assessment FROM students WHERE login = '{login}' AND password = '{password}';")
	if len(c.fetchall()) == 0:
		commitConnector(c, conn)
		return {"info":"student not found"}
	query = f"UPDATE students SET assessment='{assessment}' WHERE login = '{login}' AND password = '{password}';"
	c.execute(query)
	commitConnector(c, conn)
	return {"assessment" : assessment}


if __name__ == '__main__':
	print("Connecting to db")
	c, conn = getConnector()

	c.execute('DROP TABLE IF EXISTS students')
	c.execute('CREATE TABLE students \
	    (                   \
	      id    INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY, \
	      login  VARCHAR(30) NOT NULL UNIQUE,  \
	      password  VARCHAR(30) NOT NULL UNIQUE,  \
	      assessment  VARCHAR(30) NOT NULL \
	    )'
	)
	commitConnector(c, conn)
	print("Connected to db")
	uvicorn.run('main:app', port=8001, host='0.0.0.0', reload=True)
    
