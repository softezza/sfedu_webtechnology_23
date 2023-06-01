from typing import Optional

from pydantic import BaseModel
from tortoise.contrib.pydantic import pydantic_model_creator

from src.database.models import Goals


GoalInSchema = pydantic_model_creator(
    Goals, name="GoalIn", exclude=["author_id"], exclude_readonly=True)
GoalOutSchema = pydantic_model_creator(
    Goals, name="Goal", exclude =[
      "modified_at", "author.password", "author.created_at", "author.modified_at"
    ]
)


class UpdateGoal(BaseModel):
    title: Optional[str]
    specific_part: Optional[str]
    measureable_part: Optional[str]
    attainable_part: Optional[str]
    relevant_part: Optional[str]
    due_time: Optional[str]