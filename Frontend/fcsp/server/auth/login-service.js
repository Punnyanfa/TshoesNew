import { jwtDecode } from "jwt-decode"; // Import thư viện jwt-decode để giải mã JWT token
import { instance, Login } from "../api-instance-provider"; // Import `instance` để gọi API và `Login` cho URL
import { ElNotification } from 'element-plus';

// Hàm lưu token, email, role và userId vào localStorage
const saveTokenAndUserInfo = (token, email, role, name) => {
  if (token) {
    localStorage.setItem("userToken", token); 
    localStorage.setItem("email", email);
    localStorage.setItem("role", role);
    localStorage.setItem("name", name);

    // Giải mã token để lấy userId
    const decodedToken = jwtDecode(token);
    const userId = decodedToken.userId;

    if (userId) {
      localStorage.setItem("userId", userId); // Lưu userId vào localStorage
    }
  }
};

// Phương thức xử lý đăng nhập
export const loginUser = async (email, password) => {
  try {
    const response = await instance.post(Login.ORIGIN, { email, password });
    
    // Kiểm tra response trực tiếp từ API
    if (response.status === 200) {
      const { token, role, name } = response.data;

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

