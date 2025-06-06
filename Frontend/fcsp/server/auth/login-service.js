import { jwtDecode } from "jwt-decode"; // Import thư viện jwt-decode để giải mã JWT token
import { instance, Login } from "../api-instance-provider"; // Import `instance` để gọi API và `Login` cho URL
import { ElNotification } from 'element-plus';


const saveTokenAndUserInfo = (token, email, role) => {
  if (token) {
    // Save the token first
    localStorage.setItem("userToken", token);
    
    const decodedToken = jwtDecode(token);
    console.log("decodedToken", decodedToken);
    const userId = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
    const username = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
    const role = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    const email = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
    const  ManufacturerId= decodedToken.ManufacturerId;
    console.log("ManufacturerId", ManufacturerId);
    if (userId) {
      localStorage.setItem("userId", userId); 
    }

    if (username) {
      localStorage.setItem("username", username); 
    }
    if (role) {
      localStorage.setItem("role", role); 
    }
    if (email) {
      localStorage.setItem("email", email); 
    }
    if (ManufacturerId) {
      localStorage.setItem("ManufacturerId", ManufacturerId); 
    }
  }
};

export const loginUser = async (email, password) => {
  try {
    const response = await instance.post(Login.ORIGIN, { email, password });
    const { code, message, data } = response.data;

    if (code === 200) {
      const { token, role, name } = data;
      console.log("token", token, role, name);
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
