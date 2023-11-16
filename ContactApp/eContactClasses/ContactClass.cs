using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactApp.ContactClasses
{
    internal class ContactClass
    {
        //Just typping prop + Enter to get the implementation of method of getters and setters
        //Is like creating attrs and threre setters and getters at the same time:
        public int contactID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string contactNo { get; set; }
        public string address { get; set; }
        public string gender { get; set; }

        static string myconnstrng  = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

       public DataTable Select()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = new SqlConnection(myconnstrng))
                {
                    string sql = "SELECT * FROM tbl_contact";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            conn.Open();
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                Console.WriteLine($"Error in Select method: {ex.Message}");
            }

            return dt;
        }


        //Inserting data to data base
        public bool Insert(ContactClass c)
        {
            // No action is done before:
            bool isSuccess = false;

            try
            {
                //Need to connect to the database:
                using (SqlConnection conn = new SqlConnection(myconnstrng))
                {
                    string sql = "INSERT INTO tbl_contact (firstName, lastName, contactNo, address, gender) VALUES (@firstName, @lastName, @contactNo, @address, @gender)";
                    //Creating SQL Command using SQL and connection
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        //Creating Parameters to add data:
                        cmd.Parameters.Add("@firstName", SqlDbType.VarChar).Value = c.firstName;
                        cmd.Parameters.Add("@lastName", SqlDbType.VarChar).Value = c.lastName;
                        cmd.Parameters.Add("@contactNo", SqlDbType.VarChar).Value = c.contactNo;
                        cmd.Parameters.Add("@address", SqlDbType.VarChar).Value = c.address;
                        cmd.Parameters.Add("@gender", SqlDbType.VarChar).Value = c.gender;

                        //Then we are going to open the connection
                        conn.Open();

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Check if the query was successful
                        isSuccess = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or rethrow if needed
                Console.WriteLine(ex.Message);
            }
            // The connection will be closed in the 'finally' block or automatically due to 'using' statement
            return isSuccess;
        }

        //Method to update data in database from our application 
        public bool Update(ContactClass c)
        {
            bool isSuccess = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(myconnstrng))
                {
                    string sql = "UPDATE tbl_contact SET firstName=@firstName, lastName=@lastName, contactNo=@contactNo, address=@address, gender=@gender WHERE contactID=@contactID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@firstName", c.firstName);
                        cmd.Parameters.AddWithValue("@lastName", c.lastName);
                        cmd.Parameters.AddWithValue("@contactNo", c.contactNo);
                        cmd.Parameters.AddWithValue("@address", c.address);
                        cmd.Parameters.AddWithValue("@gender", c.gender);
                        cmd.Parameters.AddWithValue("@contactID", c.contactID);

                        conn.Open();

                        int rowsAffected = cmd.ExecuteNonQuery();

                        isSuccess = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update method: {ex.Message}");
            }

            return isSuccess;
        }


        //Method to delete from database:
        public bool Delete(ContactClass c)
        {
            //Imaginingalways that the first state is false before the success:
            bool isSuccess = false;
            //Create SQL connection:
            SqlConnection conn = new SqlConnection(myconnstrng);
            try
            {
                //SQL delete data :
                string sql = "DELETE FROM tbl_contact WHERE contactID=@contactID";
                //Create SQL command:
                SqlCommand cmd = new SqlCommand (sql,conn);
                cmd.Parameters.AddWithValue("@contactID", c.contactID);
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                //If the querry runs successfully then the value of the rows will  will be generated 
                if (rows > 0)
                {
                    isSuccess = true;
                }
                else
                {
                    isSuccess = false;
                }
            }
            catch (Exception ) 
            {

            }
            finally { 
                conn.Close();
            }

            return isSuccess;
        }

    }
}
