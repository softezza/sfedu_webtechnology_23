from fastapi import HTTPException
from tortoise.exceptions import DoesNotExist

from src.database.models import Goals
from src.schemas.goals import GoalOutSchema
from src.schemas.token import Status


async def get_goals():
    return await GoalOutSchema.from_queryset(Goals.all())


async def get_goal(goal_id) -> GoalOutSchema:
    return await GoalOutSchema.from_queryset_single(Goals.get(id=goal_id))


async def create_goal(goal, current_user) -> GoalOutSchema:
    goal_dict = goal.dict(exclude_unset=True)
    goal_dict["author_id"] = current_user.id
    goal_obj = await Goals.create(**goal_dict)
    return await GoalOutSchema.from_tortoise_orm(goal_obj)


async def update_goal(goal_id, goal, current_user) -> GoalOutSchema:
    try:
        db_goal = await GoalOutSchema.from_queryset_single(Goals.get(id=goal_id))
    except DoesNotExist:
        raise HTTPException(status_code=404, detail=f"goal {goal_id} not found")

    if db_goal.author.id == current_user.id:
        await Goals.filter(id=goal_id).update(**goal.dict(exclude_unset=True))
        return await GoalOutSchema.from_queryset_single(Goals.get(id=goal_id))

    raise HTTPException(status_code=403, detail=f"Not authorized to update")


async def delete_goal(goal_id, current_user) -> Status:
    try:
        db_goal = await GoalOutSchema.from_queryset_single(Goals.get(id=goal_id))
    except DoesNotExist:
        raise HTTPException(status_code=404, detail=f"goal {goal_id} not found")

    if db_goal.author.id == current_user.id:
        deleted_count = await Goals.filter(id=goal_id).delete()
        if not deleted_count:
            raise HTTPException(status_code=404, detail=f"goal {goal_id} not found")
        return Status(message=f"Deleted goal {goal_id}")

    raise HTTPException(status_code=403, detail=f"Not authorized to delete")