namespace Core.DataAccess
{
    public class ConnectionInfo
    {
        private string m_StrConnecion = string.Empty;
        private string m_PersistSecurity = "False";
        private string m_User = string.Empty;
        private string m_Pass = string.Empty;

        public string StrConnection
        {
            get
            {
                if (m_StrConnecion == string.Empty)
                    BuildConnection();

                return m_StrConnecion;
            }
            set { m_StrConnecion = value; }
        }

        public string DataSource { get; set; } = string.Empty;
        
        public string InitialCatalog { get; set; } = string.Empty;

        public bool PersisteSecurity
        {
            get { return bool.Parse(m_PersistSecurity); }
            set { m_PersistSecurity = value.ToString(); }
        }

        public string User
        {
            get { return m_User; }
            set { m_User = value; }
        }

        public string Password
        {
            get { return m_Pass; }
            set { m_Pass = value; }
        }

        public string Extra { get; set; } = string.Empty;

        private void BuildConnection()
        {

            m_StrConnecion = "Data Source=" + DataSource + ";";
            m_StrConnecion += "Initial Catalog=" + InitialCatalog + ";";

            if (m_User != string.Empty)
                m_StrConnecion += "User Id=" + m_User + ";";

            if (m_Pass != string.Empty)
                m_StrConnecion += "Password=" + m_Pass + ";";

            if (string.IsNullOrWhiteSpace(m_User) && string.IsNullOrWhiteSpace(m_Pass))
                m_StrConnecion += "integrated security=True;";

            //m_StrConnecion += "Persist Security Info=" + m_PersistSecurity + ";";

            m_StrConnecion += "Max Pool Size=500;";

            if (Extra != string.Empty)
                m_StrConnecion += Extra + ";";

        }
    }
}
