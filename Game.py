import random
import GameClass

game_over = False      
Player = GameClass.Player()
# 定义怪物列表
Monsters = [
    GameClass.Monster
    (
        "史莱姆",
        10,
        1,
        1,
        0.2,
        False,
        []
    ),
    GameClass.Monster
    (
        "红史莱姆",
        10,
        1,
        1,
        0.2,
        False,
        []
    ),
    GameClass.Monster
    (
        "蓝史莱姆",
        10,
        1,
        1,
        0.2,
        False,
        []
    ),
    GameClass.Monster
    (
        "哥布林",
        15,
        1,
        2,
        0,
        False,
        []
    ),
    GameClass.Monster
    (
        "兽人",
        20,
        2,
        4,
        0,
        False,
        []
    ), 
    GameClass.Monster
    (
        "史莱姆王",
        150,
        10,
        10,
        0,
        True,
        []
    ),
    GameClass.Monster
    (
        "龙",
        500,
        10,
        50,
        0,
        True,
        []
    )
    ]
