using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    //Gameplay CODE
    public Transform Goal;
    float Speed = 1.0f;
    float Accuracy = 2.0f;
    float RotSpeed = 0.6f;
    private float PI = 3.14159265359f;
    // Start is called before the first frame update
    void 
    Start()
    {
    }

    // Update is called once per frame
    void 
    LateUpdate()
    {   
        Vector3 LookAtGoal = new Vector3(Goal.position.x,
                                         this.transform.position.y,
                                         Goal.position.z);
        Vector3 Direction = LookAtGoal - this.transform.position;
        this.transform.rotation = SlerpQua(this.transform.rotation,
                                           Quaternion.LookRotation(Direction),
                                           Time.deltaTime*RotSpeed);

        if(DistanceV3(transform.position, LookAtGoal) > Accuracy)
        { 
            this.transform.Translate(0, 0, Speed*Time.deltaTime);
        }
    }





    // Manuto Math Library...
    float
    LengthV3(Vector3 V)
    {
        return Mathf.Sqrt(V.x*V.x + V.y*V.y + V.z*V.z);
    }

    Vector3
    NormalizeV3(Vector3 V)
    {
        Vector3 Result = new Vector3(V.x/LengthV3(V),
                                     V.y/LengthV3(V),
                                     V.z/LengthV3(V));
        return Result;
    }

    float 
    DistanceV3(Vector3 A, Vector3 B)
    {
        Vector3 Result = B - A;
        return LengthV3(Result);
    }
    
    float
    DotV3(Vector3 A, Vector3 B)
    {
        return (A.x*B.x + A.y*B.y + A.z*B.z);
    }
    
    Vector3
    CrossV3(Vector3 A, Vector3 B)
    {
        Vector3 Result = new Vector3();
        Result.x = A.y*B.z + A.z*B.y; 
        Result.y = A.z*B.x + A.x*B.z; 
        Result.z = A.x*B.y + A.y*B.x;
        return Result; 
    }

    float
    ToRadians(float Value)
    {
        return Value * (PI/180.0f);
    }

    float
    ToDegree(float Value)
    {
        return Value * (180.0f/PI);
    }

    float
    AngleBetweenInDegreeV3(Vector3 A, Vector3 B)
    {
        float Result = Mathf.Acos(DotV3(A, B)/(LengthV3(A)*LengthV3(B)));
        return ToDegree(Result);
    }
    
    void
    LookAtTragetV3(Vector3 V)
    {
        V = NormalizeV3(V);
        Vector3 DefaultObjectDir = new Vector3(0.0f, 0.0f, 1.0f); 
        float AngleToRotate = AngleBetweenInDegreeV3(DefaultObjectDir, V);
        if(Goal.position.x <= this.transform.position.x)
        {
            AngleToRotate = 360.0f - AngleToRotate;
        }
        Quaternion target = Quaternion.Euler(0.0f, AngleToRotate, 0.0f);
        this.transform.rotation = target;
    }

    float
    LengthQua(Quaternion Q)
    {
        return Mathf.Sqrt(Q.x*Q.x + Q.y*Q.y + Q.z*Q.z + Q.w*Q.w);
    }

    Quaternion 
    NormalizeQua(Quaternion Q)
    {
        Quaternion Result = new Quaternion(Q.x/LengthQua(Q),
                                           Q.y/LengthQua(Q),
                                           Q.z/LengthQua(Q),
                                           Q.w/LengthQua(Q));
        return Result;
    }

    Quaternion
    SlerpQua(Quaternion A, Quaternion B, float Blend)
    {
        Quaternion Result = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        float Dot = A.w*B.w + A.x*B.x + A.y*B.y + A.z*B.z;
        float BlendInvers = 1.0f - Blend;
        if(Dot < 0.0f)
        {
            Result.w = BlendInvers * A.w + Blend * -B.w;
            Result.x = BlendInvers * A.x + Blend * -B.x;
            Result.y = BlendInvers * A.y + Blend * -B.y;
            Result.z = BlendInvers * A.z + Blend * -B.z;
        }
        else
        {
            Result.w = BlendInvers * A.w + Blend * B.w;
            Result.x = BlendInvers * A.x + Blend * B.x;
            Result.y = BlendInvers * A.y + Blend * B.y;
            Result.z = BlendInvers * A.z + Blend * B.z; 
        }
        return NormalizeQua(Result);   
    } 
}
