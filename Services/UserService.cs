using System.Collections.Generic;

namespace oidc_aspcore.Services {

    public class User {
        public string _id { get; set; }
        public string loginId { get; set; }
        public string password { get; set; }
    }
    public interface IUserService {
        User getUser(string loginId, string password);
    }

    public class DummyUserService : IUserService {
        private List<User> _listUser;
        public DummyUserService() {
            _listUser = new List<User>();
            this.register("user01", "password");
            this.register("user02", "passpass");
            this.register("user03", "wordword");
        }
        public void register(string _loginId, string _password) {
            this._listUser.Add(new User() {
                _id = _listUser.Count.ToString(),
                    loginId = _loginId,
                    password = hash(_password),
            });
        }

        public string hash(string password) {
            // danger
            return password + "3";
        }

        public User getUser(string loginId, string password) {
            return this._listUser.Find(user => user.loginId == loginId && user.password == hash(password));
        }
    }
}