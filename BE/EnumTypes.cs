﻿namespace BE
{
    //ToDo: למלא את הערכים של הenum
    public enum RequestStatus
    {
        פתוחה,
        נסגרה_דרך_האתר,
        נסגרה_כי_פג_תוקפה
    }

    public enum Regions
    {
        
    }

    public enum Type
    {
    }

    public enum OrderStatus
    {
        טרם_טופל,
        נשלח_מייל,
        נסגר_מחוסר_הענות_של_הלקוח,
        נסגר_בהיענות_של_הלקוח,
        נסגר_בעקבות_סגירת_עסקה_עם_מארח_אחר
    }

    public enum Pool
    {
    }

    public enum HotTub // jacuzzi
    {
    }

    public enum Garden
    {
    }

    public enum ChildrensAttractions
    {
    }

    public enum MaxPrice//todo:להוסיף את זה לדרישת לקוח ולכל השאר
    {
        LOW = 200,
        MEDIUM = 500,
        HIGH = 1000,
        EXPENSIVE = 1500
    }
}