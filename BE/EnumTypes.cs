namespace BE
{
    public enum RequestStatus
    {
        פתוחה,
        נסגרה_דרך_האתר,
        נסגרה_כי_פג_תוקפה
    }

    public enum Regions
    {
        צפון,
        מרכז,
        דרום,
        ירושלים
    }

    public enum GRType
    {
        מלון,
        קמפינג,
        אכסניה,
        צימר,
    }

    public enum OrderStatus
    {
        טרם_טופל,
        נשלח_מייל,
        נסגר_מחוסר_הענות_של_הלקוח,
        נסגר_בהיענות_של_הלקוח,
        נסגר_בעקבות_סגירת_עסקה_עם_מארח_אחר
    }

    public enum Requirements
    {
        הכרחי,
        אפשרי,
        לא_מעוניין
    }
    

    public enum MaxPrice
    {
        Low = 200,
        MEDIUM = 500,
        HIGH = 1000,
        EXPENSIVE = 1500
    }
}