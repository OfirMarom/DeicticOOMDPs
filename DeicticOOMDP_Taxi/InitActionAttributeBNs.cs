using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DeicticOOMDP
{
    public class InitActionAttributeBNs
    {
        

        public static object Init1(int action, Type classType, PropertyInfo classAttribute)
        {
            if (classType == typeof(Taxi))
            {

                ActionAttributeBN<Taxi, int> actionAttributeBN1 = new ActionAttributeBN<Taxi, int>(3,
                EffectFuncs.Relative,
                new Func<Taxi, State, bool>[] { CondFuncs.WallNorthOfMe, CondFuncs.WallEastOfMe, CondFuncs.WallSouthOfMe, CondFuncs.WallWestOfMe, CondFuncs.TaxiOnAnyPassenger, CondFuncs.TaxiOnAnyDestination, CondFuncs.AnyPassengerInTaxi });
                actionAttributeBN1.ConjunctionTM = new ConjunctionTM(new int[] { 0, 1, 2 });  
                return new ActionAttributeBNs<Taxi, int>(actionAttributeBN1);


            }
            else if (classType == typeof(Wall))
            {

                ActionAttributeBN<Wall, int> actionAttributeBN1 = new ActionAttributeBN<Wall, int>(3, 
                    EffectFuncs.Relative, 
                    new Func<Wall, State, bool>[] { CondFuncs.TaxiNorthOfAnyWall, CondFuncs.TaxiEastOfAnyWall, CondFuncs.TaxiSouthOfAnyWall, CondFuncs.TaxiWestOfAnyWall, CondFuncs.TaxiOnAnyPassenger, CondFuncs.TaxiOnAnyDestination, CondFuncs.AnyPassengerInTaxi });
                actionAttributeBN1.ConjunctionTM = new ConjunctionTM(new int[] { 0, 1, 2 });
                return new ActionAttributeBNs<Wall, int>(actionAttributeBN1);

            }
            else if (classType == typeof(Passenger))
            {
                if (classAttribute.PropertyType == typeof(int))
                {
                    ActionAttributeBN<Passenger, int> actionAttributeBN1 = new ActionAttributeBN<Passenger, int>(3, 
                        EffectFuncs.Relative, 
                        new Func<Passenger, State, bool>[] { CondFuncs.TaxiNorthOfAnyWall, CondFuncs.TaxiEastOfAnyWall, CondFuncs.TaxiSouthOfAnyWall, CondFuncs.TaxiWestOfAnyWall, CondFuncs.TaxiOnAnyPassenger, CondFuncs.TaxiOnAnyDestination, CondFuncs.AnyPassengerInTaxi });
                    actionAttributeBN1.ConjunctionTM = new ConjunctionTM(new int[] { 0, 1, 2 });
                    return new ActionAttributeBNs<Passenger, int>(actionAttributeBN1);

                }
                else if (classAttribute.PropertyType == typeof(bool) && classAttribute.Name == "InTaxi")
                {
                    if (action == Actions.PICKUP)
                    {
                        ActionAttributeBN<Passenger, bool> actionAttributeBN1 = new ActionAttributeBN<Passenger, bool>(2, 
                            EffectFuncs.Flip, 
                            new Func<Passenger, State, bool>[] { CondFuncs.TaxiNorthOfAnyWall, CondFuncs.TaxiEastOfAnyWall, CondFuncs.TaxiSouthOfAnyWall, CondFuncs.TaxiWestOfAnyWall, CondFuncs.TaxiOnMe, CondFuncs.AtDestinationMe, CondFuncs.AnyPassengerInTaxi });
                        actionAttributeBN1.ConjunctionTM = new ConjunctionTM(new int[] { 1 });
                        return new ActionAttributeBNs<Passenger, bool>(actionAttributeBN1);
                    }
                    else if (action == Actions.DROPOFF)
                    {
                        ActionAttributeBN<Passenger, bool> actionAttributeBN1 = new ActionAttributeBN<Passenger, bool>(2,
                            EffectFuncs.Flip,
                            new Func<Passenger, State, bool>[] { CondFuncs.TaxiNorthOfAnyWall, CondFuncs.TaxiEastOfAnyWall, CondFuncs.TaxiSouthOfAnyWall, CondFuncs.TaxiWestOfAnyWall, CondFuncs.TaxiOnAnyPassenger, CondFuncs.TaxiOnAnyDestination, CondFuncs.InTaxiMe });
                        actionAttributeBN1.ConjunctionTM = new ConjunctionTM(new int[] { 1 });
                        return new ActionAttributeBNs<Passenger, bool>(actionAttributeBN1);

                    }
                    else
                    {
                        ActionAttributeBN<Passenger, bool> actionAttributeBN1 = new ActionAttributeBN<Passenger, bool>(2,
                            EffectFuncs.Flip,
                            new Func<Passenger, State, bool>[] { CondFuncs.TaxiNorthOfAnyWall, CondFuncs.TaxiEastOfAnyWall, CondFuncs.TaxiSouthOfAnyWall, CondFuncs.TaxiWestOfAnyWall, CondFuncs.TaxiOnAnyPassenger, CondFuncs.TaxiOnAnyDestination, CondFuncs.AnyPassengerInTaxi });
                        actionAttributeBN1.ConjunctionTM = new ConjunctionTM(new int[] { 0, 1 });
                        return new ActionAttributeBNs<Passenger, bool>(actionAttributeBN1);

                    }
                }
                else if (classAttribute.PropertyType == typeof(bool) && classAttribute.Name == "AtDestination")
                {
                    if (action == Actions.DROPOFF)
                    {
                        ActionAttributeBN<Passenger, bool> actionAttributeBN1 = new ActionAttributeBN<Passenger, bool>(2, 
                            EffectFuncs.Flip, 
                            new Func<Passenger, State, bool>[] { CondFuncs.TaxiNorthOfAnyWall, CondFuncs.TaxiEastOfAnyWall, CondFuncs.TaxiSouthOfAnyWall, CondFuncs.TaxiWestOfAnyWall, CondFuncs.TaxiOnAnyPassenger, CondFuncs.TaxiOnAnyDestination ,CondFuncs.InTaxiMe});
                        actionAttributeBN1.ConjunctionTM = new ConjunctionTM(new int[] { 1 });
                        return new ActionAttributeBNs<Passenger, bool>(actionAttributeBN1);

                    }
                    else
                    {
                        ActionAttributeBN<Passenger, bool> actionAttributeBN1 = new ActionAttributeBN<Passenger, bool>(2,
                          EffectFuncs.Flip,
                          new Func<Passenger, State, bool>[] { CondFuncs.TaxiNorthOfAnyWall, CondFuncs.TaxiEastOfAnyWall, CondFuncs.TaxiSouthOfAnyWall, CondFuncs.TaxiWestOfAnyWall, CondFuncs.TaxiOnAnyPassenger, CondFuncs.TaxiOnAnyDestination, CondFuncs.AnyPassengerInTaxi });
                        actionAttributeBN1.ConjunctionTM = new ConjunctionTM(new int[] { 0,1 });
                        return new ActionAttributeBNs<Passenger, bool>(actionAttributeBN1);

                    }
                }
            }
            else if (classType == typeof(Destination))
            {
                ActionAttributeBN<Destination, int> actionAttributeBN1 = new ActionAttributeBN<Destination, int>(3,
                    EffectFuncs.Relative,
                    new Func<Destination, State, bool>[] { CondFuncs.TaxiNorthOfAnyWall, CondFuncs.TaxiEastOfAnyWall, CondFuncs.TaxiSouthOfAnyWall, CondFuncs.TaxiWestOfAnyWall, CondFuncs.TaxiOnAnyPassenger, CondFuncs.TaxiOnAnyDestination, CondFuncs.AnyPassengerInTaxi });
                actionAttributeBN1.ConjunctionTM = new ConjunctionTM(new int[] { 0,1, 2 });
                return new ActionAttributeBNs<Destination, int>(actionAttributeBN1);
            }

            return null;
        }




        public static object TesTTM(int action, Type classType, PropertyInfo classAttribute)
        {
            if (classType == typeof(Taxi))
            {

                ActionAttributeBN<Taxi, int> actionAttributeBN1 = new ActionAttributeBN<Taxi, int>(3,
                EffectFuncs.Relative,
                new Func<Taxi, State, bool>[] { CondFuncs.WallNorthOfMe, CondFuncs.WallEastOfMe, CondFuncs.WallSouthOfMe, CondFuncs.WallWestOfMe });
                actionAttributeBN1.ConjunctionTM = new ConjunctionTM(new int[] { 0, 1, 2 });

                /*
                ActionAttributeBN<Taxi, int> actionAttributeBN2 = new ActionAttributeBN<Taxi, int>(11,
                EffectFuncs.Absolute,
                new Func<Taxi, State, bool>[] { PropositionFuncs.WallNorthOfMe, PropositionFuncs.WallEastOfMe, PropositionFuncs.WallSouthOfMe, PropositionFuncs.WallWestOfMe });
                actionAttributeBN2.ConjunctionTM = new ConjunctionTM(new int[] { 0,1,2,3,4,5,6,7,8,9,10});
                
                return new ActionAttributeBNs<Taxi, int>(actionAttributeBN1, actionAttributeBN2);
                */

                return new ActionAttributeBNs<Taxi, int>(actionAttributeBN1);


            }
            else if (classType == typeof(Wall))
            {

                return new ActionAttributeBNs<Wall, int>(
                    new ActionAttributeBN<Wall, int>(3, EffectFuncs.Relative, new Func<Wall, State, bool>[] { })
                );
            }
            else if (classType == typeof(Passenger))
            {
                if (classAttribute.PropertyType == typeof(int))
                {
                    return new ActionAttributeBNs<Passenger, int>(
                        new ActionAttributeBN<Passenger, int>(3, EffectFuncs.Relative, new Func<Passenger, State, bool>[] { })
                    );

                }
                else if (classAttribute.PropertyType == typeof(bool) && classAttribute.Name == "InTaxi")
                {
                    if (action == Actions.PICKUP)
                    {
                        ActionAttributeBN<Passenger, bool> actionAttributeBN1 = new ActionAttributeBN<Passenger, bool>(2, EffectFuncs.Flip, new Func<Passenger, State, bool>[] { CondFuncs.TaxiOnMe, CondFuncs.AtDestinationMe, CondFuncs.AnyPassengerInTaxi, CondFuncs.WallNorthOfMe, CondFuncs.WallEastOfMe, CondFuncs.WallSouthOfMe, CondFuncs.WallWestOfMe });
                        actionAttributeBN1.ConjunctionTM = new ConjunctionTM(new int[] { 1 });

                        ActionAttributeBN<Passenger, bool> actionAttributeBN2 = new ActionAttributeBN<Passenger, bool>(2, EffectFuncs.SetBool, new Func<Passenger, State, bool>[] { CondFuncs.TaxiOnMe, CondFuncs.AtDestinationMe, CondFuncs.AnyPassengerInTaxi, CondFuncs.WallNorthOfMe, CondFuncs.WallEastOfMe, CondFuncs.WallSouthOfMe, CondFuncs.WallWestOfMe });
                        actionAttributeBN2.ConjunctionTM = new ConjunctionTM(new int[] { });

                        return new ActionAttributeBNs<Passenger, bool>(actionAttributeBN1, actionAttributeBN2);
                    }
                    else if (action == Actions.DROPOFF)
                    {
                        return new ActionAttributeBNs<Passenger, bool>(
                            new ActionAttributeBN<Passenger, bool>(2, EffectFuncs.Flip, new Func<Passenger, State, bool>[] { CondFuncs.InTaxiMe, CondFuncs.TaxiOnAnyDestination })
                        );
                    }
                    else
                    {
                        return new ActionAttributeBNs<Passenger, bool>(
                            new ActionAttributeBN<Passenger, bool>(2, EffectFuncs.Flip, new Func<Passenger, State, bool>[] { })
                       );

                    }
                }
                else if (classAttribute.PropertyType == typeof(bool) && classAttribute.Name == "AtDestination")
                {
                    if (action == Actions.DROPOFF)
                    {
                        return new ActionAttributeBNs<Passenger, bool>(
                            new ActionAttributeBN<Passenger, bool>(2, EffectFuncs.Flip, new Func<Passenger, State, bool>[] { CondFuncs.InTaxiMe, CondFuncs.TaxiOnAnyDestination })
                        );
                    }
                    else
                    {
                        return new ActionAttributeBNs<Passenger, bool>(
                            new ActionAttributeBN<Passenger, bool>(2, EffectFuncs.Flip, new Func<Passenger, State, bool>[] { })
                        );

                    }
                }
            }
            else if (classType == typeof(Destination))
            {
                return new ActionAttributeBNs<Destination, int>(
                    new ActionAttributeBN<Destination, int>(3, EffectFuncs.Relative, new Func<Destination, State, bool>[] { })
                );
            }

            return null;
        }
    }
}
