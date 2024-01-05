using Sat.Recruitment.Core.DTOs;
using Sat.Recruitment.Core.Entities;
using Sat.Recruitment.Core.Enumerators;
using Sat.Recruitment.Core.Interfaces;
using System.Collections;

namespace Sat.Recruitment.Core.Extensions
{
    public static class UserServiceExt
    {
        static IEnumerable<UserType> _dbUserTypes;
        public static ArrayList _invalidUserTxtLines;
        public static IEnumerable<UserDetails> GetUsersFromFile(this IUserService userService, IUserTypeService userTypeService, string path)
        {
            List<UserDetails> users = new List<UserDetails>();
            StreamReader reader = ReadUsersFromFile(path);
            _dbUserTypes = userTypeService.GetUserTypes();
            _invalidUserTxtLines = new ArrayList();

            while (reader.Peek() >= 0)
            {
                var line = reader.ReadLineAsync().Result;
                var user = ConvertToUserEntity(line);
                users.Add(user);
            }

            reader.Close();
            userService.InvalidUserTxtLines = _invalidUserTxtLines;

            return users;
        }
        public static decimal CalculateGiftMoney(this IUserService userService, int userTypeId, decimal userMoneyAmount)
        {
            return CalculateGiftMoney(userTypeId, userMoneyAmount);
        }
        private static decimal CalculateGiftMoney(int userTypeId, decimal userMoneyAmount)
        {
            decimal totalAmountGift = 0;

            switch ((UserTypeEnum)userTypeId)
            {
                case UserTypeEnum.Normal:
                    if (userMoneyAmount > 100)
                        totalAmountGift = (userMoneyAmount * Convert.ToDecimal(0.12)) + userMoneyAmount;
                    else if (userMoneyAmount < 100 && userMoneyAmount > 10)
                        totalAmountGift = (userMoneyAmount * Convert.ToDecimal(0.8)) + userMoneyAmount;
                    break;
                case UserTypeEnum.Premium:
                    if (userMoneyAmount > 100)
                        totalAmountGift = (userMoneyAmount * 2) + userMoneyAmount;
                    break;
                case UserTypeEnum.SuperUser:
                    if (userMoneyAmount > 100)
                        totalAmountGift = (userMoneyAmount * Convert.ToDecimal(0.20)) + userMoneyAmount;
                    break;
                default:
                    totalAmountGift = userMoneyAmount;
                    break;
            }

            return totalAmountGift;
        }
        private static StreamReader ReadUsersFromFile(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Open);
            StreamReader reader = new StreamReader(fileStream);           

            return reader;
        }
        private static UserDetails ConvertToUserEntity(string line)
        {
            UserDetails userEntity = null;
            UserType dbUserType = null;

            //Using Trim() we can discard those empty lines
            string[] userInfo = line.Trim().Split(',');

            //According to the "Users.txt" file, we have 6 values separated by comma
            if (userInfo.Length == 6 && IsValidUserLine(userInfo))
            {                
                dbUserType = _dbUserTypes.Where(x => x.TypeName.ToLower().Contains(userInfo[4].ToString().Trim().ToLower())).FirstOrDefault();
                if (dbUserType != null)
                    userEntity = new UserDetails
                    {
                        Name = userInfo[0].ToString(),
                        Email = userInfo[1].ToString(),
                        Phone = userInfo[2].ToString().Replace("+", ""),
                        Address = userInfo[3].ToString(),
                        UserTypeId = dbUserType.Id,
                        Money = CalculateGiftMoney(dbUserType.Id, decimal.Parse(userInfo[5].ToString())),
                    };
            }

            return userEntity;
        }
        private static bool IsValidUserLine(string[] userInfo)
        {
            bool isValid = true;

            foreach (string item in userInfo)
                if (isValid = !string.IsNullOrEmpty(item))
                {
                    _invalidUserTxtLines.Add(item);
                    break;
                }

            return isValid;
        }
    }
}
