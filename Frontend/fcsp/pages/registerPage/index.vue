<template>
  <div class="register-container">
    <div class="register-box">
      <!-- Phần trái: Hình ảnh sneaker -->
      <div class="register-image">
        <img src="https://th.bing.com/th/id/OIP.hmwhi4IHiABkRdfZSbxQ2AHaHa?w=160&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7" alt="Sneaker Image" />
      </div>

      <!-- Phần phải: Form đăng ký -->
      <div class="register-form">
        <h1 class="brand-name">Create Your Account</h1>
        <p class="subtitle">Join us to design your custom sneakers</p>

        <form @submit.prevent="register">
          <div class="mb-3">
            <label class="form-label">Name</label>
            <div class="input-group">
              <UserOutlined class="input-icon" />
              <input v-model="name" type="text" class="form-control" :class="{ 'is-invalid': nameError }" placeholder="Enter your name" required />
            </div>
            <div v-if="nameError" class="error-message">{{ nameError }}</div>
          </div>

          <div class="mb-3">
            <label class="form-label">Email</label>
            <div class="input-group">
              <MailOutlined class="input-icon" />
              <input v-model="email" type="email" class="form-control" :class="{ 'is-invalid': emailError }" placeholder="Enter your email" required />
            </div>
            <div v-if="emailError" class="error-message">{{ emailError }}</div>
          </div>

          <div class="mb-3">
            <label class="form-label">Password</label>
            <div class="input-group">
              <LockOutlined class="input-icon" />
              <input v-model="password" :type="showPassword ? 'text' : 'password'" class="form-control" :class="{ 'is-invalid': passwordError }" placeholder="Create a password" required />
              <button type="button" class="toggle-password" @click="togglePassword">
                <EyeOutlined v-if="!showPassword" />
                <EyeInvisibleOutlined v-else />
              </button>
            </div>
            <div v-if="passwordError" class="error-message">{{ passwordError }}</div>
          </div>

          <button type="submit" class="btn btn-custom w-100">Sign Up</button>
        </form>

        <p class="text-center mt-3">
          Already have an account? <router-link to="/loginPage" class="link">Login</router-link>
        </p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { registerUser } from '@/server/auth/register-service';
import {
  UserOutlined,
  MailOutlined,
  LockOutlined,
  EyeOutlined,
  EyeInvisibleOutlined
} from '@ant-design/icons-vue';

const router = useRouter();
const name = ref('');
const email = ref('');
const password = ref('');
const showPassword = ref(false);
const nameError = ref('');
const emailError = ref('');
const passwordError = ref('');

const validateInputs = () => {
  let isValid = true;
  
  // Validate name
  if (!name.value.trim()) {
    nameError.value = 'Name is required';
    isValid = false;
  } else {
    nameError.value = '';
  }

  // Validate email
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!email.value.trim()) {
    emailError.value = 'Email is required';
    isValid = false;
  } else if (!emailRegex.test(email.value)) {
    emailError.value = 'Please enter a valid email address';
    isValid = false;
  } else {
    emailError.value = '';
  }

  // Validate password
  if (!password.value) {
    passwordError.value = 'Password is required';
    isValid = false;
  } else if (password.value.length < 6) {
    passwordError.value = 'Password must be at least 6 characters';
    isValid = false;
  } else {
    passwordError.value = '';
  }

  return isValid;
};

const togglePassword = () => {
  showPassword.value = !showPassword.value;
};

const register = async () => {
  try {
    if (!validateInputs()) return;

    // Gọi API với đúng thứ tự tham số (name, email, password)
    const response = await registerUser(
      name.value,    // tham số 1: name
      email.value,   // tham số 2: email
      password.value 
      
    );console.log("register response:", name.value);

    

    // Xử lý response thành công
    if (response) {
      // Có thể hiển thị thông báo thành công
      alert('Registration successful!');
      // Chuyển hướng đến trang login
      router.push('/loginPage');
    }
  } catch (error) {
    // Xử lý các trường hợp lỗi chi tiết hơn
    if (error.response) {
      // Lỗi từ server với status code
      switch (error.response.status) {
        case 400:
          alert('Invalid input data. Please check your information.');
          break;
        case 409:
          alert('Email already exists. Please use a different email.');
          break;
        case 500:
          alert('Server error. Please try again later.');
          break;
        default:
          alert(`Registration failed: ${error.response.data.message || 'Please try again later.'}`);
      }
    } else if (error.request) {
      // Lỗi không nhận được response
      alert('Unable to connect to the server. Please check your internet connection.');
    } else {
      // Lỗi khác
      alert('Registration failed: ' + error.message);
    }
    console.error('Registration error:', error);
  }
};
</script>

<style scoped>
/* 🌟 Background */
.register-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background: #f5f5f5;
  border-radius: none;
}

/* 📦 Register Box (Chia 2 cột) */
.register-box {
  display: flex;
  width: 100%;
  height: 100vh;
  background: white;
  border-radius: 0;
  box-shadow: none;
  overflow: hidden;
}

/* 🎨 Phần phải: Form đăng ký */
.register-form {
  width: 50%;
  padding: 4rem;
  display: flex;
  flex-direction: column;
  justify-content: center;
}

/* 🔥 Branding */
.brand-name {
  font-size: 2rem;
  font-weight: bold;
  color: #ff5722;
  text-transform: uppercase;
  text-align: center;
}

.subtitle {
  font-size: 1rem;
  color: #555;
  margin-bottom: 20px;
  text-align: center;
}

/* 🔑 Input Group */
.input-group {
  display: flex;
  align-items: center;
  position: relative;
}

.input-icon {
  position: absolute;
  left: 10px;
  color: #ff5722;
  font-size: 20px;
  z-index: 1;
}

input {
  width: 100%;
  padding-left: 40px;
  border: none;
  border-radius: 0;
  transition: 0.3s ease-in-out;
  background-color: #f5f5f5;
}

input:focus {
  border: none;
  box-shadow: none;
  background-color: #ebebeb;
  outline: none;
}

/* 👁️ Toggle Password */
.toggle-password {
  position: absolute;
  right: 10px;
  background: none;
  border: none;
  cursor: pointer;
  font-size: 20px;
}

/* ❌ Error Message */
.error-message {
  color: red;
  font-size: 0.875rem;
  text-align: left;
  margin-top: 5px;
}

/* 🚀 Custom Button */
.btn-custom {
  background: #ff5722;
  color: white;
  font-weight: bold;
  text-transform: uppercase;
  transition: 0.3s;
  border: none;
  border-radius: 0;
  padding: 12px;
}

.btn-custom:hover {
  background: #e64a19;
  transform: scale(1.05);
}

/* 📎 Sign Up Link */
.link {
  color: #ff5722;
  text-decoration: none;
  font-weight: bold;
}

.link:hover {
  text-decoration: underline;
}

/* 🏀 Phần trái: Hình ảnh sneaker */
.register-image {
  width: 50%;
  height: 100vh;
  background: linear-gradient(135deg, #ff5722, #ff8a50);
  display: flex;
  justify-content: center;
  align-items: center;
}

.register-image img {
  width: 90%;
  border-radius: 10px;
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.3);
}

/* Thêm media query cho responsive */
@media (max-width: 768px) {
  .register-box {
    flex-direction: column;
  }
  
  .register-form,
  .register-image {
    width: 100%;
  }
  
  .register-image {
    height: 40vh;
  }
}
</style>
