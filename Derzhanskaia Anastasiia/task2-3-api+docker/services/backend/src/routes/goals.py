from typing import List

from fastapi import APIRouter, Depends, HTTPException
from tortoise.contrib.fastapi import HTTPNotFoundError
from tortoise.exceptions import DoesNotExist

import src.crud.goals as crud
from src.auth.jwthandler import get_current_user
from src.schemas.goals import GoalOutSchema, GoalInSchema, UpdateGoal
from src.schemas.token import Status
from src.schemas.users import UserOutSchema


router = APIRouter()


@router.get(
    "/goals",
    response_model=List[GoalOutSchema],
    dependencies=[Depends(get_current_user)],
)
async def get_goals():
    return await crud.get_goals()


@router.get(
    "/goal/{goal_id}",
    response_model=GoalOutSchema,
    dependencies=[Depends(get_current_user)],
)
async def get_goal(goal_id: int) -> GoalOutSchema:
    try:
        return await crud.get_goal(goal_id)
    except DoesNotExist:
        raise HTTPException(
            status_code=404,
            detail="Goal does not exist",
        )


@router.post(
    "/goals", response_model=GoalOutSchema, dependencies=[Depends(get_current_user)]
)
async def create_goal(
    goal: GoalInSchema, current_user: UserOutSchema = Depends(get_current_user)
) -> GoalOutSchema:
    return await crud.create_goal(goal, current_user)


@router.patch(
    "/goal/{goal_id}",
    dependencies=[Depends(get_current_user)],
    response_model=GoalOutSchema,
    responses={404: {"model": HTTPNotFoundError}},
)
async def update_goal(
    goal_id: int,
    goal: UpdateGoal,
    current_user: UserOutSchema = Depends(get_current_user),
) -> GoalOutSchema:
    return await crud.update_goal(goal_id, goal, current_user)


@router.delete(
    "/goal/{goal_id}",
    response_model=Status,
    responses={404: {"model": HTTPNotFoundError}},
    dependencies=[Depends(get_current_user)],
)
async def delete_goal(
    goal_id: int, current_user: UserOutSchema = Depends(get_current_user)
):
    return await crud.delete_goal(goal_id, current_user)
