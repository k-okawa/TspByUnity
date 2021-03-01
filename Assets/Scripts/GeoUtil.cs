using System;

public static class GeoUtil {
    /// <summary>
    /// 緯度経度間の距離を取得する
    /// </summary>
    /// <param name="lat1">緯度1</param>
    /// <param name="lng1">経度1</param>
    /// <param name="lat2">緯度2</param>
    /// <param name="lng2">経度2</param>
    /// <param name="precision">小数点以下の桁数（距離の精度)</param>
    /// <returns>距離(メートル)</returns>
    public static double GeoDistance(double lat1, double lng1, double lat2, double lng2, int precision) {
        double distance = 0;
        if ((Math.Abs(lat1 - lat2) < 0.00001) && (Math.Abs(lng1 - lng2) < 0.00001)) {
            distance = 0;
        } else {
            lat1 = lat1 * Math.PI / 180;
            lng1 = lng1 * Math.PI / 180;
            lat2 = lat2 * Math.PI / 180;
            lng2 = lng2 * Math.PI / 180;
            double A = 6378140;
            double B = 6356755;
            double F = (A - B) / A;
            double P1 = Math.Atan((B / A) * Math.Tan(lat1));
            double P2 = Math.Atan((B / A) * Math.Tan(lat2));
            double X = Math.Acos(Math.Sin(P1) * Math.Sin(P2) + Math.Cos(P1) * Math.Cos(P2) * Math.Cos(lng1 - lng2));
            double L = (F / 8) * ((Math.Sin(X) - X) * Math.Pow((Math.Sin(P1) + Math.Sin(P2)), 2) / Math.Pow(Math.Cos(X / 2), 2) - (Math.Sin(X) - X) * Math.Pow(Math.Sin(P1) - Math.Sin(P2), 2) / Math.Pow(Math.Sin(X), 2));
            distance = A * (X + L);
            double decimal_no = Math.Pow(10, precision);
            distance = Math.Round(decimal_no * distance / 1) / decimal_no;
        }
        return distance;
    }
}