BasicAttackSubState: 關閉WASD的旋轉，使主控角色的普通攻擊能自動轉向敵人。可開啟技能。普攻動作結束時，普攻計數增加，如果下個動作不是普攻或閃避，要將普攻計數歸零。離開普攻SubStateMachine時，關閉普通攻擊特效，關閉攻擊判定偵測。
HitSubState: 受擊，關閉WASD跟自動轉向敵人的旋轉，要根據攻擊來源方向做受擊動作。不可開啟技能。
LocomotionSubState: 關閉自動轉向敵人的旋轉，可開啟技能。
PutBackWeapon: 關閉武器。
SkillSubState: 關閉WASD的旋轉，使主控角色的技能能自動轉向敵人。不可開啟技能。離開技能SubStateMachine時，關閉技能特效，關閉攻擊判定偵測。
TakeOutWeapon: 開啟武器。
WsadRotatable: 開啟WASD的旋轉。
