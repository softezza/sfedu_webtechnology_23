from fastapi import FastAPI
from datetime import date
from pydantic import BaseModel
from json import JSONEncoder
import json
import uvicorn
import mysql.connector as mc

def getCursor():
	conn= mc.connect(host='db',user='root',password='12345',db='vpn_db')
	c = conn.cursor() 
	return c,conn
	
def commit(c, conn):
	conn.commit()
	c.close()
	conn.close()


print("Connecting to db")
c, conn = getCursor()

c.execute('DROP TABLE IF EXISTS users')
c.execute('CREATE TABLE users \
    (                   \
      id    INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY, \
      login  VARCHAR(30) NOT NULL UNIQUE,  \
      password  VARCHAR(30) NOT NULL UNIQUE,  \
      vpn_mode  VARCHAR(30) NOT NULL \
    )'
)
commit(c, conn)
print("Connected to db")

app = FastAPI()


class UserMode(BaseModel):
    login: str
    password: str
    mode: str
    
class User(BaseModel):
    login: str
    password: str
    

@app.get("/")
def index():
	return {"Hello, world!"}
	
@app.get("/mode/")
def getMode(credentials: User):
	login = credentials.login
	password = credentials.password
	c, conn = getCursor()
	c.execute(f"SELECT  vpn_mode FROM users WHERE login = '{login}' AND password = '{password}'")
	mode = c.fetchall()
	if len(mode) == 0:
		commit(c, conn)
		return {"info":"user not found"}
	return {"mode" : mode[0][0]}
	
@app.post("/update_mode/")
def updateMode(credentials: UserMode):
	login = credentials.login
	password = credentials.password
	mode = credentials.mode
	c, conn = getCursor()
	c.execute(f"SELECT  vpn_mode FROM users WHERE login = '{login}' AND password = '{password}';")
	if len(c.fetchall()) == 0:
		commit(c, conn)
		return {"info":"user not found"}
	query = f"UPDATE users SET vpn_mode='{mode}' WHERE login = '{login}' AND password = '{password}';"
	c.execute(query)
	commit(c, conn)
	return {"mode" : mode}

@app.post("/sign_up/")
def registerUser(credentials: User):
	login = credentials.login
	password = credentials.password
	c, conn = getCursor()
	c.execute(f"SELECT  * FROM users WHERE login = '{login}' OR password = '{password}'")
	if len(c.fetchall()) > 0:
		commit(c, conn)
		return {"registered" : False, "info":"user already created"}
	
	query = f"INSERT INTO users (login,password,vpn_mode) VALUES('{login}','{password}','standart');"
	c.execute(query)
	commit(c, conn)
	return {"registered" : True}
	
@app.delete("/unregister/")
def unregisterUser(credentials: User):
	login = credentials.login
	password = credentials.password
	c, conn = getCursor()
	c.execute(f"SELECT  * FROM users WHERE login = '{login}' AND password = '{password}'")
	if len(c.fetchall()) == 0:
		commit(c, conn)
		return {"info":"user not found"}
	
	query = f"DELETE FROM users WHERE login = '{login}' AND password = '{password}';"
	c.execute(query)
	commit(c, conn)
	return {"unregistered" : True}




if __name__ == '__main__':
	print("Connecting to db")
	c, conn = getCursor()

	c.execute('DROP TABLE IF EXISTS users')
	c.execute('CREATE TABLE users \
	    (                   \
	      id    INT UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY, \
	      login  VARCHAR(30) NOT NULL UNIQUE,  \
	      password  VARCHAR(30) NOT NULL UNIQUE,  \
	      vpn_mode  VARCHAR(30) NOT NULL \
	    )'
	)
	commit(c, conn)
	print("Connected to db")
	uvicorn.run('main:app', port=8001, host='0.0.0.0', reload=True)
    
