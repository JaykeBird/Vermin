using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Physics_Platformer_Kit.Scripts
{
    public static class CharacterClassData  //stores player data in between scene changes
    {
        public enum characterClass { SQUIRREL, RACCOON, FERRET, BIRD };
		private static characterClass playerCharacterClass = characterClass.RACCOON; //player is a squirrel by default


        public static void setClass(characterClass newClass)
        {
            playerCharacterClass = newClass;
        }
        public static characterClass getClass()
        {
            return playerCharacterClass;
        }


        public static double getClassJump(characterClass myClass) //returns the multiplier for this class's Jump Height
        {
            if (myClass == characterClass.SQUIRREL)
            {
                return 1.1;
            }
            if (myClass == characterClass.RACCOON)
            {
                return 1;
            }
            if (myClass == characterClass.FERRET)
            {
                return 1.2;
            }
            else //if (myClass == characterClass.BIRD)
            {
                return 0.5;     //The bird is able to make small hops and jump while in the air
            }
        }
        public static double getClassSpeed(characterClass myClass) //returns the multiplier for this class's Speed
        {
            if (myClass == characterClass.SQUIRREL)
            {
                return 1.5;
            }
            if (myClass == characterClass.RACCOON)
            {
                return 0.9;
            }
            if (myClass == characterClass.FERRET)
            {
                return 1;
            }
            else //if (myClass == characterClass.BIRD)
            {
                return 1;
            }
        }
        public static int getClassCapacity(characterClass myClass) //returns the coin capacity for each class
        {
            if (myClass == characterClass.SQUIRREL)
            {
                return 5;
            }
            if (myClass == characterClass.RACCOON)
            {
                return 12;
            }
            if (myClass == characterClass.FERRET)
            {
                return 8;
            }
            else //if (myClass == characterClass.BIRD)
            {
                return 8;
            }
        }
        public static bool getClassKnockOver(characterClass myClass) //returns whether or not the class can knock over furniture
        {
            if (myClass == characterClass.RACCOON)
            {
                return true;   //only raccoon can knock over furniture
            }
            else
            {
                return false;
            }
        }
        public static bool getClassCanClimb(characterClass myClass) //returns whether or not the class can climb vertical objects
        {
            if (myClass == characterClass.SQUIRREL)
            {
                return true;   //only squirrel
            }
            else
            {
                return false;
            }
        }
        public static int getCoinLossAmount(characterClass myClass)
        {
            if (myClass == characterClass.SQUIRREL)
            {
                return 2;
            }
            else if (myClass == characterClass.RACCOON)
            {
                return 0;
            }
            else if (myClass == characterClass.FERRET)
            {
                return 1;
            }
            else //if (myClass == characterClass.BIRD)
            {
                return 1;
            }
        }
    }
}