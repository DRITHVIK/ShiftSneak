    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    //This does not create new data. It only monitors arrays to see if there are new entries / exits.
    public struct ArrayTriggerChecker<T> where T : Object
    {
        [SerializeField] private T[] curItems;
        [SerializeField] private T[] enteredItems;
        public bool IsTriggerEntered(out T[] entered) { entered = triggerEnteredItems; return this.entered; }
        public bool IsTriggerExited(out T[] exited) { exited = triggerExitedItems; return this.exited; }
        public bool IsTriggerStayed(out T[] stayed) { stayed = curItems; return this.stayed; }
        public bool IsTriggerFirst(out T[] first) { first = triggerFirstItems; return this.first; }
        public bool IsTriggerEmpty(out T[] empty) { empty = triggerEmptyItems; return this.empty; }
        public bool IsEmpty => empty;
        private bool entered;
        private bool exited;
        private bool stayed;
        private bool first;
        private bool empty;
        private T[] triggerEnteredItems;
        private T[] triggerExitedItems;
        private T[] triggerFirstItems;
        private T[] triggerEmptyItems;

        public void CheckArray(T[] array)
        {
            int length = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != null)
                    length++;
            }
            curItems = new T[length];
            int curInd = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != null)
                {
                    curItems[curInd] = array[i];
                    curInd++;
                }
            }
                
            CheckEntered();
            CheckExited();
            CheckStayed();
        }

        private void CheckEntered()
        {
            first = false;
            entered = false;
            triggerEnteredItems = new T[0];
            triggerFirstItems = new T[0];
            for (int i = 0; i < curItems.Length; i++)
            {
                bool match = false;
                for (int j = 0; j < enteredItems?.Length; j++)
                {
                    if (curItems[i].Equals(enteredItems[j]))
                        match = true;
                }
                if (!match)
                {
                    //trigger enter
                    entered = true;
                    var newEnterTrig = new T[triggerEnteredItems.Length + 1];
                    for (int j = 0; j < triggerEnteredItems.Length; j++)
                        newEnterTrig[j] = triggerEnteredItems[j];
                    newEnterTrig[newEnterTrig.Length - 1] = curItems[i];
                    triggerEnteredItems = newEnterTrig;

                    //add to entered array
                    int newInd = enteredItems != null ? enteredItems.Length : 0;
                    var newEntered = new T[newInd + 1];
                    for (int j = 0; j < enteredItems?.Length; j++)
                        newEntered[j] = enteredItems[j];
                    newEntered[newEntered.Length - 1] = curItems[i];

                    //check if first
                    if (enteredItems?.Length == 0 || enteredItems == null)
                        first = true;

                    if (first)
                        triggerFirstItems = triggerEnteredItems;

                    //store new entered
                    enteredItems = newEntered;
                }
            }
        }

        private void CheckExited()
        {
            empty = false;
            exited = false;
            triggerExitedItems = new T[0];
            triggerEmptyItems = new T[0];
            for (int i = 0; i < enteredItems?.Length; i++)
            {
                bool match = false;
                for (int j = 0; j < curItems.Length; j++)
                {
                    if (enteredItems[i].Equals(curItems[j]))
                        match = true;
                }
                if (!match)
                {
                    //trigger exit
                    exited = true;
                    var newExitTrig = new T[triggerExitedItems.Length + 1];
                    for (int j = 0; j < triggerExitedItems.Length; j++)
                        newExitTrig[j] = triggerExitedItems[j];
                    newExitTrig[newExitTrig.Length - 1] = enteredItems[i];
                    triggerExitedItems = newExitTrig;

                    //remove from entered array
                    var newEntered = new T[enteredItems.Length - 1];
                    int ind = 0;
                    for (int j = 0; j < enteredItems.Length; j++)
                    {
                        if (j != i)
                        {
                            newEntered[ind] = enteredItems[j];
                            ind++;
                        }
                            
                    }

                    //check empty
                    if (curItems.Length == 0)
                        empty = true;

                    if (empty)
                        triggerEmptyItems = triggerExitedItems;

                    enteredItems = newEntered;
                }
            }
        }

        private void CheckStayed()
        {
            stayed = false;
            if (curItems.Length > 0)
            {
                stayed = true;
            }
        }
    }
}


