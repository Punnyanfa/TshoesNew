import axios from "axios";

export const instance = axios.create({
  baseURL:
    process.env.VITE_PUBLIC_BASE_URL || "https://fcspwebapi20250527114117.azurewebsites.net/api",
});

// Thêm interceptor để xử lý headers
instance.interceptors.request.use((config) => {
  // Nếu data là FormData, để axios tự động set Content-Type: multipart/form-data
  if (config.data instanceof FormData) {
    config.headers['Content-Type'] = 'multipart/form-data';
  } else {
    // Mặc định là application/json
    config.headers['Content-Type'] = 'application/json';
  }
  // Thêm Authorization nếu có token
  const token = localStorage.getItem('userToken');
  if (token) {
    config.headers['Authorization'] = `Bearer ${token}`;
  }
  return config;
});

export const Login = {
  ORIGIN: "Auth/Login", // Đường dẫn API login
};
export const Register = {
  ORIGIN: "Auth/Register", // Đường dẫn API register
};
export const BestSelling = {
  ORIGIN: "CustomShoeDesign/top-5-best-selling", 
};