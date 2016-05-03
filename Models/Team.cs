using System;
using System.Collections;
using System.Collections.Generic;

namespace QwkThx
{
	public class Team
	{
		public List<TeamPersonnel> Staff = new List<TeamPersonnel>();

		public Team(){
			Staff.Add (new TeamPersonnel("person1", "Person One"));
			Staff.Add (new TeamPersonnel("person2", "Person Two"));
			Staff.Add (new TeamPersonnel("person3", "Person Three"));
			Staff.Add (new TeamPersonnel("person4", "Person Four"));
		}
	}

	public class TeamPersonnel
	{
		public string username;
		public string fullname;
		public string role;

		public TeamPersonnel(string username, string fullname){
			this.username = username;
			this.fullname = fullname;
		}

		public override string ToString(){
			return username;
		}
	}

}

