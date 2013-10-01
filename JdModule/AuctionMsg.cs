using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JdModule
{
    class AuctionMsg
    {
        public static string GetMsg(string code)
        {
            switch (code)
            {
                case "200":
                    return "出价成功!";
                case "453":
                    return "出价失败，同一用户不能连续出价!";
                case "451":
                    return "出价失败，出价不得低于当前价格!";
                case "452":
                    return "当前拍卖尚未开始，不能出价!";
                case "450":
                    return "当前拍卖已经结束，不能出价!";
                case "457":
                    return "您的京东积分不足，不能出价！";
                case "459":
                    return "出价失败，出价不能低于商品起拍价!";
                case "460":
                    return "出价失败，每次加价不得低于最低加价!";
                case "461":
                    return "出价失败，每次加价不得高于最高加价!";
                case "462":
                    return "出价失败，出价不能超过京东价!";
                case "463":
                    return "出价失败，出价格式不正确,拍卖出价应为正整数!";
                case "464":
                    return "出价失败，该拍卖已关闭!";
                case "465":
                    return "出价失败，该拍卖已删除!";
                case "466":
                    return "出价失败，您的账户异常!";
                case "468":
                    return "出价异常!";
                case "469":
                    return "尊敬的京东会员,您暂无参拍资格[请确认参拍资格,普通拍卖请确认积分大于0]！";
                case "470":
                    return "尊敬的京东会员,您的积分需要大于等于0才可参与拍卖！";
                case "4201":
                    return "出价失败，同时在拍的商品不得超过10个!";
                case "4202":
                    return "出价失败，未支付的拍卖不得超过10个!";
                case "4203":
                    return "出价失败，同时在拍的商品和未支付的不得超过10个!";
                case "400":
                    return "出价失败，同一时刻用户出价失败!";
                case "403":
                    return "对不起，您尚未登录，请在登录后进行拍卖出价。";
                case "402":
                    return "系統异常，请稍后再试！";
            }
            return null;
        }
    }
}
