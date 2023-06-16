from fastapi import FastAPI

app = FastAPI()

@app.get("/")
async def root():
    return {"message": "Hello World"}
    
@app.get("/sign_in")
async def root():
    return {"message": "sign_in"}
    
@app.get("/sign_up")
async def root():
    return {"message": "sign_up"}
