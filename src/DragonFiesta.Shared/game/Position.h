#ifndef POSITION_H
#define POSITION_H

#include "../Define.h"
#include <math.h>
#include <memory>
#include <cstdlib>
#include "Util/randomc.h"

const float epsilon = 0.001f;


struct Position
{

    uint16 X;
    uint16 Y;
    int8   Rotation;

    Position() { }
    Position(Position& pBase)
    {
        X = pBase.X;
        Y = pBase.Y;
    }
    Position(uint16 _X,uint16 _Y, int8 _Rotation) : Position()
    {
        X = _X;
        Y = _Y;
        Rotation = _Rotation;
    }


    Position(uint16 _X,uint16 _Y)
    {
        X = _X;
        Y = _Y;
    }

    float DistanceSquared(Position value1, Position value2)
    {
        float num2 = value1.X - value2.X;
        float num = value1.Y - value2.Y;
        return ((num2 * num2) + (num * num));
    }

    float Distance(Position value1, Position value2)
    {
        float num2 = value1.X - value2.X;
        float num = value1.Y - value2.Y;
        float num3 = (num2 * num2) + (num * num);
        return (float)sqrt((double)num3);
    }

//fix later
    /*
        Position GetRandomSpotAround(Position pBase, int16 pRadius)
        {
            int xmin = pBase.X - pRadius, xmax = pBase.X + pRadius;
            int ymin = pBase.Y - pRadius, ymax = pBase.Y + pRadius;
    //        return new Position(ymin,xmin);
    //        return Position(IRandom((xmin, xmax), IRandom((ymin, ymax));
        }*/
    Position& operator = ( const Position& otherpoint )
    {
        this->X = otherpoint.X;
        this->Y = otherpoint.Y;
        this->Rotation = otherpoint.Rotation;
        return *this;
    }
    Position& operator += ( const Position& otherpoint )
    {
        this->X += otherpoint.X;
        this->Y += otherpoint.Y;
        this->Rotation += otherpoint.Rotation;
        return *this;
    }
    Position& operator -= ( const Position& otherpoint )
    {
        this->X -= otherpoint.X;
        this->Y -= otherpoint.Y;
        this->Rotation -= otherpoint.Rotation;
        return *this;
    }
    Position& operator *= ( const Position& otherpoint )
    {
        this->X *= otherpoint.X;
        this->Y *= otherpoint.Y;
        this->Rotation *= otherpoint.Rotation;
        return *this;
    }
    Position& operator /= ( const Position& otherpoint )
    {
        this->X /= otherpoint.X;
        this->Y /= otherpoint.Y;
        this->Rotation /= otherpoint.Rotation;
        return *this;
    }
    Position& operator *= ( float mul )
    {
        this->X *= mul;
        this->Y *= mul;
        this->Rotation *= mul;
        return *this;
    }
    bool operator == ( const Position& otherpoint )
    {
        return (fabs(this->X-otherpoint.X)<epsilon && fabs(this->Y-otherpoint.Y)<epsilon && fabs(this->Rotation-otherpoint.Rotation)<epsilon);
    }
    bool operator != ( const Position& otherpoint )
    {
        return !(*this==otherpoint);
    }



};





#endif
