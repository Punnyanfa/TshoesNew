import { instance, Register } from "../api-instance-provider";

// Hàm lưu thông tin người dùng và token vào localStorage
const saveTokenAndUserInfo = (token, email, role, name) => {
  localStorage.setItem('userToken', token);
  localStorage.setItem('userEmail', email);
  localStorage.setItem('userRole', role);
  localStorage.setItem('userName', name);
};

export const registerUser = async (name, email, password) => {
  try {
    const response = await instance.post(Register.ORIGIN, { name, email, password });
    console.log("registerUser response", response);
    
    if (response.status === 200) {
      const { token, email, role, name } = response.data;
      saveTokenAndUserInfo(token, email, role, name);  // Lưu thông tin vào localStorage
      return response.data;
    }

    throw new Error("Failed to register: " + response.statusText);
  } catch (error) {
    // Log toàn bộ lỗi để dễ dàng kiểm tra
    console.error('Registration error details:', error);

    if (error.response) {
      console.error('Error response:', error.response);
      throw new Error(`Failed to register: ${error.response.data?.message || 'Unknown error'}`);
    } else if (error.request) {
      console.error('Error request:', error.request);
      throw new Error('Failed to register: No response received');
    } else {
      console.error('Error message:', error.message);
      throw new Error(`Failed to register: ${error.message}`);
    }
  }
};
