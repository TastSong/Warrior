/***************************************************************
 * Class Name : GoalManager
 * Function   : Manages the goals for an agent.Calculates their relevancy
 *				and decides when to build/rebuild plans.Also steps through them.
 *				Also responsibile for  building plans. Run AStar and create plan from nodes
 *				
 * Created by : Marek R.
 **************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;


class GOAPManager : System.Object
{
	private	List<GOAPGoal>	m_GoalSet = new List<GOAPGoal> ();

	public GOAPGoal CurrentGoal = null;
// { get { return CurrentGoal; } private set { CurrentGoal = value; } }
	private Agent Owner;

	private AStarEngine AStar;
	private AStarStorage Storage;
	private AStarGOAPMap Map;
	private AStarGOAPGoal Goal;

	public GOAPManager (Agent ai)
	{
		Owner = ai;
		Map = new AStarGOAPMap ();//Initialise the AStar Planner
		//Map.Initialise(Owner);
		//Map.BuildActionsEffectsTable();//Build the action effects table 
	
		Storage = new AStarStorage ();

		Goal = new AStarGOAPGoal ();
	
		AStar = new AStarEngine ();

		//AStar.Setup(Goal,Storage,Map);
	}

	public void Initialize ()
	{
		Map.Initialise (Owner);
		Map.BuildActionsEffectsTable ();//Build the action effects table 
		AStar.Setup (Goal, Storage, Map);
	}

	/**
 * Reset the goal manager after a run
 */
	public void Reset ()
	{
		if (CurrentGoal != null) {
			CurrentGoal.Deactivate ();
			CurrentGoal = null;
		}

		for (int i = 0; i < m_GoalSet.Count; i++)
			m_GoalSet [i].Reset ();
	}


	public void Clean ()
	{
		AStar.Cleanup ();
		AStar = null;

		//mapPlanner->Cleanup();
		Map = null;

		//storage->Cleanup();
		Storage = null;

		//goalPlanner->Cleanup();
		Goal = null;
	}


	/**
	* Adds the goal to the list of goals
	* @param the new goal
	*/
	public GOAPGoal AddGoal (E_GOAPGoals newGoal)
	{
		if (!IsGoalInGoalSet (newGoal)) {
			GOAPGoal goal = GOAPGoalFactory.Create (newGoal, Owner);
			m_GoalSet.Add (goal);
			return goal;	
		}
		return null;
	}

	/**
	* Checks whether the goal to be added is part of the agents goals already
	* @return true if the goal is part of the agent's goalset
	*/
	bool IsGoalInGoalSet (E_GOAPGoals goalType)
	{
		for (int i = 0; i < m_GoalSet.Count; i++) {
			if (m_GoalSet [i].GoalType == goalType)
				return true;
		}
		return false;
	}

	

	/**
 * Updates the current goal
 */
	public void UpdateCurrentGoal ()
	{
		if (CurrentGoal != null) {
			if (CurrentGoal.UpdateGoal ()) {
				if (CurrentGoal.ReplanRequired ()) {
					ReplanCurrentGoal ();
				}

				if (CurrentGoal.IsPlanFinished ()) {// goal is finished, so clear it and make new one

					CurrentGoal.Deactivate ();
					CurrentGoal = null;
				}
			} else { 
				CurrentGoal.Deactivate ();
				CurrentGoal = null;
			}
		}
	}

  

	public void ManageGoals ()
	{	// first try to find critical goal !!!
		//First check if the current plan is invalid.
		//Then check if the current plan has validated the WS conditions
		//If this is so, then we need to find a new relevant goal and set that to the current goal

		if (FindCriticalGoal ())
			return;

		if (CurrentGoal != null) {
			if (CurrentGoal.ReplanRequired ()) {
				//if (Owner.debugGOAP) Debug.Log(Time.timeSinceLevelLoad + " " +  CurrentGoal.ToString() + " - REPLAN required !!");

				if (ReplanCurrentGoal () == false) {
					//if (Owner.debugGOAP) Debug.Log(Time.timeSinceLevelLoad + " " + CurrentGoal.ToString() + " - REPLAN failed, find new goal");
                    
					FindNewGoal ();
				}
			} else if (!CurrentGoal.IsPlanValid ()) {	//Current goal's plan is invalid, replan and update goals flags set

				//if (Owner.debugGOAP) Debug.Log(Time.timeSinceLevelLoad + " " + CurrentGoal.ToString() + " - INVALID, find new goal");
                
				FindNewGoal ();
			} else if (CurrentGoal.IsSatisfied ()) {//	Current goal's goal WS has been satisfied, replan and update goals flags set
				//if (Owner.debugGOAP) Debug.Log(Time.timeSinceLevelLoad + " " + CurrentGoal.ToString() + " - DONE, find new goal");
                
				FindNewGoal ();
			} else if (CurrentGoal.IsPlanInterruptible ()) {
				FindMostImportantGoal ();
			}
		} else {
			FindNewGoal ();
		}

	}

	bool ReplanCurrentGoal ()
	{
		if (CurrentGoal == null)
			return false;

		CurrentGoal.ReplanReset ();

		GOAPPlan plan = BuildPlan (CurrentGoal);

		if (plan == null) {
			//if (Owner.debugGOAP) Debug.Log(Time.timeSinceLevelLoad + " " + CurrentGoal.ToString() + " - REPLAN failed");
			return false;
		}
    
		CurrentGoal.Activate (plan);

		return true;

	}

	void FindNewGoal ()
	{
		if (CurrentGoal != null) {
			CurrentGoal.Deactivate ();
			CurrentGoal = null;
		}

		GOAPGoal newGoal = GetMostImportantGoal (0);
		if (newGoal == null) {
			//if (Owner.debugGOAP) Debug.Log(" Find new goal: none" );
			return;
		}

		/*if (Owner.debugGOAP)
        {
            Debug.Log(" Find new goal: " + newGoal.ToString());

            Debug.Log(Owner.WorldState.ToString());

            for (int i = 0; i < m_GoalSet.Count; i++)
            {
                Debug.Log(m_GoalSet[i].ToString());
            }
        }*/


		CreatePlan (newGoal);
	}

	void FindMostImportantGoal ()
	{
		GOAPGoal newGoal = GetMostImportantGoal (CurrentGoal.GoalRelevancy);
		if (newGoal == null)
			return;

		if (newGoal == CurrentGoal) {
			//if (Owner.debugGOAP) Debug.Log(Time.timeSinceLevelLoad + " Current goal " + CurrentGoal.ToString() + ": " + "is most important still (" + newGoal.GoalRelevancy + ")");
			return;
		}

        
		//if (Owner.debugGOAP) Debug.Log(Time.timeSinceLevelLoad + " " + CurrentGoal.ToString() + ": " + newGoal.ToString() + " is more important (" + newGoal.GoalRelevancy + ")");

		/*if (Owner.debugGOAP)
        {
            Debug.Log("FindMostImportantGoal: " + newGoal.ToString());
            if (Owner.debugGOAP) Debug.Log(Owner.WorldState.ToString());

            for (int i = 0; i < m_GoalSet.Count; i++)
            {
                Debug.Log(m_GoalSet[i].ToString());
            }
        }*/


		if (CurrentGoal != null) {
			CurrentGoal.Deactivate ();
			CurrentGoal = null;
		}

		CreatePlan (newGoal);
	}

	public bool FindCriticalGoal ()
	{
		GOAPGoal newGoal = GetCriticalGoal ();
		if (newGoal == null)
			return false;

		if (newGoal == CurrentGoal) {
			//if (Owner.debugGOAP) Debug.Log(Time.timeSinceLevelLoad + " Current goal " + CurrentGoal.ToString() + ": " + "is most important still (" + newGoal.GoalRelevancy + ")");
			return false;
		}

		/*if (Owner.debugGOAP)
        {
            Debug.Log("CRITICAL GOAL: " + newGoal.ToString());

            if (Owner.debugGOAP) Debug.Log(Owner.WorldState.ToString());

            for (int i = 0; i < m_GoalSet.Count; i++)
            {
                Debug.Log(m_GoalSet[i].ToString());
            }
        }*/

		if (CurrentGoal != null) {
			CurrentGoal.Deactivate ();
			CurrentGoal = null;
		}

		CreatePlan (newGoal);

		return true;
	}


	void CreatePlan (GOAPGoal goal)
	{
		GOAPPlan plan = BuildPlan (goal);
		if (plan == null) {
			Debug.LogError (Time.timeSinceLevelLoad + " " + goal.ToString () + " - BUILD PLAN FAILED !!!");
			return;
		}

		CurrentGoal = goal;
		CurrentGoal.Activate (plan);       
	}


	GOAPGoal GetMostImportantGoal (float minRelevancy)
	{
		GOAPGoal maxGoal = null;
		float highestRelevancy = minRelevancy;

		GOAPGoal goal;
		for (int i = 0; i < m_GoalSet.Count; i++) {	//First check for timing checks?!

			goal = m_GoalSet [i];
			if (goal.IsDisabled ())
				continue;//we dont want to select these goals

			if (highestRelevancy >= goal.GetMaxRelevancy ())
				continue; // nema cenu resit goal ktery ma mensi prioritu nez uz vybrany

			if (goal.Active == false) {// recalculate goal relevancy !!!!
				goal.CalculateGoalRelevancy ();
				//m_GoalSet[i].SetNewEvaluationTime();
				//set new timing check time?!
			}

			// check all goal relevancy
			if (goal.GoalRelevancy > highestRelevancy) {
				highestRelevancy = goal.GoalRelevancy;
				maxGoal = goal;
			}

		}

		return maxGoal;
	}

	GOAPGoal GetCriticalGoal ()
	{
		GOAPGoal maxGoal = null;
		float highestRelevancy = 0.0f;

		float goalRelevance = 0.0f;

		if (CurrentGoal != null && CurrentGoal.Active)
			goalRelevance = CurrentGoal.GoalRelevancy;

		for (int i = 0; i < m_GoalSet.Count; i++) {	//First check for timing checks?!

			if (m_GoalSet [i].IsDisabled ())
				continue;//we dont want to select these goals

			if (m_GoalSet [i].Critical == false)
				continue;

			if (m_GoalSet [i].Active == false) {// recalculate goal relevancy !!!!
				m_GoalSet [i].CalculateGoalRelevancy ();
				//m_GoalSet[i].SetNewEvaluationTime();
				//set new timing check time?!
			}

			// check all goal relevancy
			goalRelevance = m_GoalSet [i].GoalRelevancy;
			if (goalRelevance > highestRelevancy) {
				highestRelevancy = goalRelevance;
				maxGoal = m_GoalSet [i];
			}
		}
		return maxGoal;
	}


	/**
	* Gets the most relevant goal at the moment
	*
    GOAPGoal GetMostRelevantGoal(bool recalculate)
    {
        GOAPGoal maxGoal = null;
        float highestRelevancy = 0.0f;
        float currentTime = Time.timeSinceLevelLoad;
        float nextEvalTime;

        float goalRelevance = 0.0f;
        for (int i = 0; i < m_GoalSet.Count; i++)
        {	//First check for timing checks?!
            //Don't recalculate the goal relevancy if not asked to do so

            if (recalculate)
            {
                nextEvalTime = m_GoalSet[i].NextEvaluationTime;

                if (currentTime < nextEvalTime)
                { //clear relevancy , we dont want to select these goals
                    m_GoalSet[i].ClearGoalRelevancy();
                }
                else if (!m_GoalSet[i].GetReEvalOnSatisfication() && m_GoalSet[i].IsWSSatisfied(Ai.GetWorldState()))
                {
                    m_GoalSet[i].ClearGoalRelevancy();
                }
                else
                {// recalculate goal relevancy !!!!
                    m_GoalSet[i].CalculateGoalRelevancy();
                    //m_GoalSet[i].SetNewEvaluationTime();
                    //set new timing check time?!

                }
            }
            // check all goal relevancy
            goalRelevance = m_GoalSet[i].GoalRelevancy;
            if (goalRelevance > highestRelevancy)
            {
                highestRelevancy = goalRelevance;
                maxGoal = m_GoalSet[i];
            }

        }
        return maxGoal;
    }*/
	/**
	* Builds a new plan for this agent
	* @param the agent to build the plan for
	* @return true if the plan builds successfully, false otherwise
	*/

	public GOAPPlan BuildPlan (GOAPGoal goal)
	{
		if (goal == null)
			return null;

		//if (Owner.debugGOAP) Debug.Log(Time.timeSinceLevelLoad + " " + goal.ToString() + " - build plan");

		//initialize shit
		Map.Initialise (Owner);
		Goal.Initialise (Owner, Map, goal);

		Storage.ResetStorage (Map);

		AStar.End = -1;
		AStar.RunAStar (Owner);

		AStarNode currNode = AStar.CurrentNode;

		if (currNode == null || currNode.NodeID == -1) {
			Debug.LogError (Time.timeSinceLevelLoad + " " + goal.ToString () + " - FAILED , no node ");
			return null;		//Building of plan failed
		}

		GOAPPlan plan = new GOAPPlan (); //create a new plan

		GOAPAction action;
		/**
		 * We need to push each new plan step onto the list of steps until we reach the last action
		 * Which is going to be the goal node and of no use
		 */

		//if (Owner.debugGOAP) Debug.Log(Time.timeSinceLevelLoad + " " + goal.ToString() + " current node id :" + currNode.NodeID);

		while (currNode.NodeID != -1) {
			action = Map.GetAction (currNode.NodeID);

			if (action == null) {//If we tried to cast an node to an action that can't be done, quit out
				Debug.LogError (Time.timeSinceLevelLoad + " " + goal.ToString () + ": canot find action (" + currNode.NodeID + ")");
				return null;
			}

			plan.PushBack (action);
			currNode = currNode.Parent;
		}

		//Finally tell the ai what its plan is
		if (plan.IsDone ()) {
			Debug.LogError (Time.timeSinceLevelLoad + " " + goal.ToString () + ": plan is already  done !!! (" + plan.CurrentStepIndex + "," + plan.NumberOfSteps + ")");
			return null;
		}

		return plan;
	}
}