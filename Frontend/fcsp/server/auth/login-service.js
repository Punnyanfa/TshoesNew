import { jwtDecode } from "jwt-decode"; // Import thư viện jwt-decode để giải mã JWT token
import { instance, Login } from "../api-instance-provider"; // Import `instance` để gọi API và `Login` cho URL
import { ElNotification } from 'element-plus';

// Hàm lưu token, email, role và userId vào localStorage
// Hàm lưu token, email, role, userId và username vào localStorage
const saveTokenAndUserInfo = (token, email, role) => {
  if (token) {
    localStorage.setItem("userToken", token); 
    localStorage.setItem("email", email);
    localStorage.setItem("role", role);

    // Giải mã token để lấy thông tin
    const decodedToken = jwtDecode(token);
    console.log("decodedToken", decodedToken);
    const userId = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
    const username = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
    console.log("username", username);
    if (userId) {
      localStorage.setItem("userId", userId); // Lưu userId vào localStorage
    }

    if (username) {
      localStorage.setItem("username", username); // Lưu username vào localStorage
    }
  }
};

// Phương thức xử lý đăng nhập
export const loginUser = async (email, password) => {
  try {
    const response = await instance.post(Login.ORIGIN, { email, password });
    const { code, message, data } = response.data;
    // Kiểm tra response trực tiếp từ API
    if (code === 200) {
      const { token, role, name } = data;
      console.log("token", token, role, name);
      // Lưu thông tin user
      saveTokenAndUserInfo(token, email, role, name);

      if (role === "Admin") {
        ElNotification({
          title: 'Success',
          message: 'Admin login successful!',
          type: 'success',
        });
        return "Admin login";
      }

      ElNotification({
        title: 'Success',
        message: 'Login successful!',
        type: 'success',
      });
      return "Login successful";
    }

    ElNotification({
      title: 'Error',
      message: 'Login failed',
      type: 'error',
    });
    return "Failed";

  } catch (error) {
    console.error('Login error:', error);
    
    ElNotification({
      title: 'Error',
      message: error.response?.data?.message || 'Login failed! Please check your email and password.',
      type: 'error',
    });
    return "Error";
  }
};
