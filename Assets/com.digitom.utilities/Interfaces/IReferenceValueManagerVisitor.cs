using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitomUtilities
{
    public interface IReferenceValueManagerVisitor
    {
        IReferenceValue[] GetValueReferences(ReferenceValueManager _manager);
    }
}


