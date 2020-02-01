//
// /**************************************************************************
//
// SkillData.cs
//
// Author: xiaohong  <704872627@qq.com>
//
// Unity课程讨论群:  152767675
//
// Date: 2016/8/9
//
// Description:Provide  functions  to connect Oracle
//
// Copyright (c) 2016 xiaohong
//
// **************************************************************************/
using System;

public class ComboStep
{
	public E_AttackType AttackType;
	public E_ComboLevel ComboLevel;
	public AnimAttackData Data;
}

public class Combo
{
	public E_SwordLevel SwordLevel;
	public ComboStep[] ComboSteps;
}

public class SkillData
{
	public static Combo[] PlayerComboAttacks = new Combo[6];

	public static void Init(AnimSetPlayer AnimSet)
	{
		PlayerComboAttacks[0] = new Combo() // FAST   Raisin Wave
		{
			SwordLevel = E_SwordLevel.One,
			ComboSteps = new ComboStep[]{new ComboStep(){AttackType = E_AttackType.X, ComboLevel = E_ComboLevel.One, Data = AnimSet.AttackData[0]},
				new ComboStep(){AttackType = E_AttackType.X, ComboLevel = E_ComboLevel.One, Data = AnimSet.AttackData[1]},
				new ComboStep(){AttackType = E_AttackType.X, ComboLevel = E_ComboLevel.One, Data = AnimSet.AttackData[2]},
				new ComboStep(){AttackType = E_AttackType.X, ComboLevel = E_ComboLevel.Two, Data = AnimSet.AttackData[3]},
				new ComboStep(){AttackType = E_AttackType.O, ComboLevel = E_ComboLevel.Three, Data = AnimSet.AttackData[4]},
			}
		};
		PlayerComboAttacks[1] = new Combo() // BREAK BLOCK  half moon
		{
			SwordLevel = E_SwordLevel.One,
			ComboSteps = new ComboStep[]{new ComboStep(){AttackType = E_AttackType.O, ComboLevel = E_ComboLevel.One, Data = AnimSet.AttackData[5]},
				new ComboStep(){AttackType = E_AttackType.O, ComboLevel = E_ComboLevel.One, Data = AnimSet.AttackData[6]},
				new ComboStep(){AttackType = E_AttackType.O, ComboLevel = E_ComboLevel.One, Data = AnimSet.AttackData[7]},
				new ComboStep(){AttackType = E_AttackType.X, ComboLevel = E_ComboLevel.Two, Data = AnimSet.AttackData[8]},
				new ComboStep(){AttackType = E_AttackType.X, ComboLevel = E_ComboLevel.Three, Data = AnimSet.AttackData[9]},
			}
		};
		PlayerComboAttacks[2] = new Combo() // CRITICAL  cloud cuttin
		{
			SwordLevel = E_SwordLevel.Two,
			ComboSteps = new ComboStep[]{new ComboStep(){AttackType = E_AttackType.O, ComboLevel = E_ComboLevel.One, Data = AnimSet.AttackData[5]},
				new ComboStep(){AttackType = E_AttackType.O, ComboLevel = E_ComboLevel.One, Data = AnimSet.AttackData[6]},
				new ComboStep(){AttackType = E_AttackType.X, ComboLevel = E_ComboLevel.One, Data = AnimSet.AttackData[17]},
				new ComboStep(){AttackType = E_AttackType.X, ComboLevel = E_ComboLevel.Two, Data = AnimSet.AttackData[18]},
				new ComboStep(){AttackType = E_AttackType.X, ComboLevel = E_ComboLevel.Three, Data = AnimSet.AttackData[19]},
			}
		};

		PlayerComboAttacks[3] = new Combo()  // flying dragon
		{
			SwordLevel = E_SwordLevel.Three,
			ComboSteps = new ComboStep[]{new ComboStep(){AttackType = E_AttackType.X, ComboLevel = E_ComboLevel.One, Data = AnimSet.AttackData[0]},
				new ComboStep(){AttackType = E_AttackType.O, ComboLevel = E_ComboLevel.One, Data = AnimSet.AttackData[10]},
				new ComboStep(){AttackType = E_AttackType.O, ComboLevel = E_ComboLevel.One, Data = AnimSet.AttackData[11]},
				new ComboStep(){AttackType = E_AttackType.X, ComboLevel = E_ComboLevel.Two, Data = AnimSet.AttackData[12]},
				new ComboStep(){AttackType = E_AttackType.X, ComboLevel = E_ComboLevel.Three, Data = AnimSet.AttackData[13]},
			}
		};
		PlayerComboAttacks[4] = new Combo() // KNCOK //walking death
		{
			SwordLevel = E_SwordLevel.Four,
			ComboSteps = new ComboStep[]{new ComboStep(){AttackType = E_AttackType.X, ComboLevel = E_ComboLevel.One, Data = AnimSet.AttackData[0]},
				new ComboStep(){AttackType = E_AttackType.X, ComboLevel = E_ComboLevel.One, Data = AnimSet.AttackData[1]},
				new ComboStep(){AttackType = E_AttackType.O, ComboLevel = E_ComboLevel.One, Data = AnimSet.AttackData[14]},
				new ComboStep(){AttackType = E_AttackType.X, ComboLevel = E_ComboLevel.Two, Data = AnimSet.AttackData[15]},
				new ComboStep(){AttackType = E_AttackType.X, ComboLevel = E_ComboLevel.Three, Data = AnimSet.AttackData[16]},
			}
		};

		PlayerComboAttacks[5] = new Combo() // HEAVY, AREA  shogun death
		{
			SwordLevel = E_SwordLevel.Five,
			ComboSteps = new ComboStep[]{new ComboStep(){AttackType = E_AttackType.O, ComboLevel = E_ComboLevel.One, Data = AnimSet.AttackData[5]},
				new ComboStep(){AttackType = E_AttackType.X, ComboLevel = E_ComboLevel.One, Data = AnimSet.AttackData[20]},
				new ComboStep(){AttackType = E_AttackType.O, ComboLevel = E_ComboLevel.One, Data = AnimSet.AttackData[21]},
				new ComboStep(){AttackType = E_AttackType.O, ComboLevel = E_ComboLevel.Two, Data = AnimSet.AttackData[22]},
				new ComboStep(){AttackType = E_AttackType.O, ComboLevel = E_ComboLevel.Three, Data = AnimSet.AttackData[23]},
			}
		};

	}
}


