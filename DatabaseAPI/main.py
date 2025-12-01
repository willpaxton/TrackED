from fastapi import FastAPI
from pydantic import BaseModel

import psycopg2

@app.get("/")
def read_root():
    return {"Hello": "World (v2)"}