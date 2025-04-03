import axios from "axios";

export const instance = axios.create({
  baseURL:
    process.env.VITE_PUBLIC_BASE_URL || "https://wyp-somee-api.somee.com/api",
    headers: {
      'Content-Type': 'application/json',  // Đảm bảo header Content-Type được gửi đúng
    },
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