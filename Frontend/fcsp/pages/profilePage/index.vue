<template>
  <Header />
  <div class="container-fluid">
    <div class="container py-4">
      <div class="row">
        <div class="col-12">
          <h2 class="mb-0">My Profile</h2>
          <p class="text-muted">Manage your profile information to secure your account</p>
          <hr class="my-3" />
        </div>
      </div>
      <div class="row">
        <div class="col-md-8">
          <!-- Nút Update Profile -->
          <div class="mb-3 row">
            <div class="col-sm-9 offset-sm-3">
              <button type="button" class="button-btn" @click="showModal = true">Update Profile</button>
            </div>
          </div>
          <!-- Thông tin profile -->
          <div class="mb-3 row">
            <label for="fullName" class="col-sm-3 col-form-label text-md-end">Full Name</label>
            <div class="col-sm-9">
              <span class="me-2">{{ profile.fullName }}</span>
            </div>
          </div>
          <div class="mb-3 row">
            <label class="col-sm-3 col-form-label text-md-end">Email:</label>
            <div class="col-sm-9 d-flex align-items-center">
              <span class="me-2">{{ profile.email }}</span>
            </div>
          </div>
          <div class="mb-3 row">
            <label class="col-sm-3 col-form-label text-md-end">Phone Number:</label>
            <div class="col-sm-9 d-flex align-items-center">
              <span class="me-2">{{ profile.phone }}</span>
            </div>
          </div>
          <div class="mb-3 row">
            <label class="col-sm-3 col-form-label text-md-end">Gender</label>
            <div class="col-sm-9">
              <span class="me-2">{{ profile.gender }}</span>
            </div>
          </div>
          <div class="mb-3 row">
            <label class="col-sm-3 col-form-label text-md-end">Date of Birth</label>
            <div class="col-sm-9 d-flex align-items-center">
              <span class="me-2">{{ profile.birthdate }}</span>
            </div>
          </div>
          <div class="mb-3 row">
            <div class="col-sm-9 offset-sm-3">
              <button type="button" class="button-btn" @click="showChangePassword = true">Change Password</button>
            </div>
          </div>
        </div>
        <div class="col-md-4 text-center">
          <div class="d-flex flex-column align-items-center">
            <div class="position-relative mb-3">
              <img
                :src="profile.avatarImageUrl || 'https://hebbkx1anhila5yf.public.blob.vercel-storage.com/image-Pg8Xar0cs5YJAv7GozxRFA8TXVJ7tP.png'"
                alt="Profile Picture"
                class="rounded-circle img-thumbnail"
                style="width: 150px; height: 150px; object-fit: cover"
              />
            </div>
            <button class="btn btn-outline-secondary mb-2" @click="selectImage">Choose Image</button>
            <input
              type="file"
              ref="fileInput"
              @change="onFileChange"
              accept=".jpeg,.jpg,.png"
              style="display: none"
            />
            <p class="text-muted small mb-1">Maximum file size 1 MB</p>
            <p class="text-muted small">Format: .JPEG, .PNG</p>
          </div>
        </div>
      </div>
    </div>
    <!-- Modal Update Profile (Bootstrap) -->
    <div class="modal fade show" tabindex="-1" :style="showModal ? 'display: block; background: rgba(0,0,0,0.5);' : 'display: none;'" aria-modal="true" role="dialog" v-if="showModal">
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Update Profile</h5>
            <button type="button" class="btn-close" @click="showModal = false" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            <form @submit.prevent="submitUpdate">
              <div class="mb-3">
                <label for="modalFullName" class="form-label">Full Name</label>
                <input type="text" class="form-control" id="modalFullName" v-model="editProfile.fullName" required />
              </div>
              <div class="mb-3">
                <label for="modalPhone" class="form-label">Phone Number</label>
                <input type="text" class="form-control" id="modalPhone" v-model="editProfile.phone" required />
              </div>
              <div class="mb-3">
                <label class="form-label">Gender</label>
                <select class="form-select" v-model="editProfile.gender" required>
                  <option value="Male">Male</option>
                  <option value="Female">Female</option>
                  <option value="Other">Other</option>
                </select>
              </div>
              <div class="mb-3">
                <label for="modalDob" class="form-label">Date of Birth</label>
                <input type="date" class="form-control" id="modalDob" v-model="editProfile.birthdate" required />
              </div>
              <div class="modal-footer">
                <button type="submit" class="btn btn-primary mx-auto">Save</button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
    <!-- Thêm backdrop khi showModal -->
    <div v-if="showModal" class="modal-backdrop"></div>
    <!-- Modal Change Password (Bootstrap) -->
    <div class="modal fade show" tabindex="-1" :style="showChangePassword ? 'display: block; background: rgba(0,0,0,0.5);' : 'display: none;'" aria-modal="true" role="dialog" v-if="showChangePassword">
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
          <div class="modal-header">
            <h6 class="modal-title">Change Password</h6>
            <button type="button" class="btn-close" @click="showChangePassword = false" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            <form @submit.prevent="submitChangePassword">
              <div class="mb-3">
                <label class="form-label">Current Password</label>
                <input type="password" class="form-control" v-model="changePassword.oldPassword" required />
              </div>
              <div class="mb-3">
                <label class="form-label">New Password</label>
                <input type="password" class="form-control" v-model="changePassword.newPassword" required />
              </div>
              <div class="mb-3">
                <label class="form-label">Confirm New Password</label>
                <input type="password" class="form-control" v-model="changePassword.confirmPassword" required />
              </div>
              <div class="modal-footer">
                <button type="submit" class="btn btn-primary mx-auto">Change</button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
    <!-- Thêm backdrop khi showChangePassword -->
    <div v-if="showChangePassword" class="modal-backdrop"></div>
  </div>
  <Footer />
</template>

<script>
import { getProfile, updateProfile, updateAvatar } from '../../server/profile-service';
import { changePassword } from '../../server/auth/senEmail-service';
export default {
  name: 'ProfilePage',
  data() {
    return {
      profile: {
        username: '',
        fullName: '',
        email: '',
        phone: '',
        gender: '',
        birthdate: '',
        avatar: null
      },
      showModal: false,
      editProfile: {
        fullName: '',
        phone: '',
        gender: '',
        birthdate: ''
      },
      showChangePassword: false,
      changePassword: {
        oldPassword: '',
        newPassword: '',
        confirmPassword: ''
      }
    }
  },
  async mounted() {
    const id = localStorage.getItem('userId');
    if (!id) {
      alert('You need to login to access this page!');
      this.$router.push('/loginPage');
      return; 
    }
    try {
      const data = await getProfile(id);
      this.profile = {
        username: data.name || '',
        fullName: data.name || '',
        email: data.email || '',
        phone: data.phoneNumber || '',
        gender: data.gender || '',
        birthdate: data.dob || '',
        avatarImageUrl: data.avatarImageUrl || '' // Nếu API có trả về avatar thì map vào đây
      };
      // Khởi tạo dữ liệu cho modal
      this.editProfile = {
        fullName: this.profile.fullName,
        phone: this.profile.phone,
        gender: this.profile.gender,
        birthdate: this.profile.birthdate
      };
    } catch (e) {
      // Xử lý lỗi nếu cần
      console.error('Failed to load profile', e);
    }
  },
  methods: {
    async submitUpdate() {
      const id = localStorage.getItem('userId');
      if (!id) {
        alert('You need to login to update your information!');
        this.$router.push('/loginPage');
        return;
      }
      try {
        const body = {
          id: Number(id),
          name: this.editProfile.fullName,
          gender: this.editProfile.gender,
          dob: this.editProfile.birthdate,
          phoneNumber: this.editProfile.phone
        };
        await updateProfile(body);
        // Cập nhật lại localStorage nếu tên mới khác tên cũ
        if (localStorage.getItem('username') !== this.editProfile.fullName) {
          localStorage.setItem('username', this.editProfile.fullName);
        }
        // Cập nhật lại profile hiển thị
        this.profile.fullName = this.editProfile.fullName;
        this.profile.phone = this.editProfile.phone;
        this.profile.gender = this.editProfile.gender;
        this.profile.birthdate = this.editProfile.birthdate;
        this.showModal = false;
        alert('Profile information has been updated!');
      } catch (e) {
        alert('Failed to update profile!');
        console.error(e);
      }
    },
    selectImage() {
      this.$refs.fileInput.click()
    },
    onFileChange(event) {
      const file = event.target.files[0]
      if (!file) return
      // Check file size (1MB = 1048576 bytes)
      if (file.size > 1048576) {
        alert('File size is too large. Please choose a file smaller than 1MB.')
        return
      }
      // Check file type
      if (!['image/jpeg', 'image/png'].includes(file.type)) {
        alert('Only JPEG or PNG files are accepted.')
        return
      }
      const id = localStorage.getItem('userId')
      if (!id) {
        alert('You need to login to update your avatar!')
        this.$router.push('/loginPage')
        return
      }
      // Tạo FormData đúng chuẩn multipart/form-data
      const formData = new FormData()
      formData.append('Id', id)
      formData.append('Avatar', file)
      // Gọi API cập nhật avatar
      updateAvatar(formData)
        .then(() => {
          alert('Avatar updated successfully!')
          // Có thể reload lại profile ở đây nếu muốn
        })
        .catch((err) => {
          alert('Failed to update avatar!')
          console.error(err)
        })
    },
    async submitChangePassword() {
      if (this.changePassword.newPassword !== this.changePassword.confirmPassword) {
        alert('New password and confirm password do not match!');
        return;
      }
      if (this.changePassword.newPassword.length < 8) {
        alert('Password must be at least 8 characters!');
        return;
      }
      const id = localStorage.getItem('userId');
      if (!id) {
        alert('You need to login to change your password!');
        this.$router.push('/loginPage');
        return;
      }
      try {
        await changePassword(Number(id), this.changePassword.oldPassword, this.changePassword.newPassword);
        alert('Password changed successfully!');
        this.showChangePassword = false;
        this.changePassword.oldPassword = '';
        this.changePassword.newPassword = '';
        this.changePassword.confirmPassword = '';
      } catch (e) {
        alert('Failed to change password! ' + (e.response?.data?.message || ''));
        console.error(e);
      }
    }
  }
}
</script>

<style scoped>
.container-fluid {
  background-color: #f8f9fa;
  padding-top: 100px;
  padding-bottom: 100px;
}
.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem;
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
  margin-top: 2rem;
  margin-bottom: 2rem;
}

h2 {
  color: #2c3e50;
  font-weight: 700;
  font-size: 2rem;
  margin-bottom: 0.5rem;
}

.text-muted {
  color: #6c757d;
  font-size: 0.9rem;
}

hr {
  border-color: #e9ecef;
  margin: 1.5rem 0;
}

.form-control {
  border: 1px solid #dee2e6;
  border-radius: 8px;
  padding: 0.75rem 1rem;
  transition: all 0.3s ease;
}

.form-control:focus {
  border-color: #AAAAAA;
  box-shadow: 0 0 0 0.2rem rgba(170, 170, 170, 0.25);
}

.form-control-plaintext {
  color: #2c3e50;
  font-weight: 500;
}

.text-primary {
  color: #AAAAAA !important;
  text-decoration: none;
  transition: color 0.3s ease;
}

.text-primary:hover {
  color: #888888 !important;
}

.form-check-input {
  cursor: pointer;
}

.form-check-input:checked {
  background-color: #AAAAAA;
  border-color: #AAAAAA;
}

.btn-danger {
  background: linear-gradient(45deg, #e74c3c, #c0392b);
  border: none;
  padding: 0.75rem 2rem;
  font-weight: 600;
  border-radius: 8px;
  transition: all 0.3s ease;
}

.btn-danger:hover {
  background: linear-gradient(45deg, #c0392b, #e74c3c);
  transform: translateY(-2px);
  box-shadow: 0 4px 15px rgba(231, 76, 60, 0.3);
}

.btn-outline-secondary {
  border: 2px solid #AAAAAA;
  color: #AAAAAA;
  padding: 0.5rem 1.5rem;
  border-radius: 8px;
  font-weight: 500;
  transition: all 0.3s ease;
}

.btn-outline-secondary:hover {
  background: #AAAAAA;
  color: white;
  transform: translateY(-2px);
}

.img-thumbnail {
  border: 3px solid #AAAAAA;
  padding: 0.25rem;
  transition: all 0.3s ease;
}

.img-thumbnail:hover {
  transform: scale(1.05);
  box-shadow: 0 4px 15px rgba(170, 170, 170, 0.3);
}

.small {
  font-size: 0.8rem;
  color: #6c757d;
}

/* Responsive styles */
@media (max-width: 768px) {
  .container {
    padding: 1rem;
    margin: 1rem;
  }

  h2 {
    font-size: 1.5rem;
  }

  .col-form-label {
    text-align: left !important;
    margin-bottom: 0.5rem;
  }

  .btn-danger {
    width: 100%;
  }
}

/* Animation for form elements */
.form-control, .btn, .form-check-input {
  transition: all 0.3s ease;
}

/* Custom radio button styles */
.form-check-input {
  width: 1.2em;
  height: 1.2em;
  margin-top: 0.2em;
}

.form-check-label {
  margin-left: 0.5rem;
  cursor: pointer;
}

/* Profile picture container */
.position-relative {
  position: relative;
  overflow: hidden;
  border-radius: 50%;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
}

/* File input styling */
input[type="file"] {
  display: none;
}

.button-btn {
  background: linear-gradient(45deg, #2c3e50, #AAAAAA);
  color: #fff;
  border: none;
  border-radius: 50px;
  padding: 12px 30px;
  font-weight: 600;
  font-size: 1rem;
  letter-spacing: 0.5px;
  text-transform: uppercase;
  transition: all 0.3s ease;
  box-shadow: 0 4px 15px rgba(170, 170, 170, 0.2);
  position: relative;
  overflow: hidden;
  cursor: pointer;
  width: 100%;
  max-width: 200px;
}

.button-btn:hover {
  background: linear-gradient(45deg, #AAAAAA, #2c3e50);
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(170, 170, 170, 0.3);
}

.button-btn:active {
  transform: translateY(0);
  box-shadow: 0 2px 10px rgba(170, 170, 170, 0.2);
}

.button-btn::before {
  content: '';
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(
    90deg,
    transparent,
    rgba(255, 255, 255, 0.2),
    transparent
  );
  transition: 0.5s;
}

.button-btn:hover::before {
  left: 100%;
}

@media (max-width: 768px) {
  .button-btn {
    max-width: 100%;
    padding: 10px 20px;
    font-size: 0.9rem;
  }
}

/* Profile section spacing */
.mb-3 {
  margin-bottom: 1.5rem !important;
}

/* Form row spacing */
.row {
  margin-bottom: 1rem;
}

/* Custom scrollbar */
::-webkit-scrollbar {
  width: 8px;
}

::-webkit-scrollbar-track {
  background: #f1f1f1;
}

::-webkit-scrollbar-thumb {
  background: #AAAAAA;
  border-radius: 4px;
}

::-webkit-scrollbar-thumb:hover {
  background: #888888;
}

/* Modern Bootstrap Modal Customization */
.modal.fade.show {
  display: block;
  z-index: 2000;
}
.modal-dialog {
  max-width: 350px;
  margin: 2rem auto;
}
.modal-content {
  border-radius: 14px;
  box-shadow: 0 4px 24px rgba(30,41,59,0.13), 0 1.5px 6px rgba(52, 152, 219, 0.06);
  padding: 0.5rem 0.5rem 0 0.5rem;
}
.modal-header {
  border-bottom: none;
  background: linear-gradient(90deg, #AAAAAA 0%, #2c3e50 100%);
  color: #fff;
  border-radius: 14px 14px 0 0;
  padding: 1rem 1.25rem 0.75rem 1.25rem;
}
.modal-title {
  font-weight: 600;
  font-size: 1.1rem;
  letter-spacing: 0.2px;
}
.modal-body {
  padding: 1rem 1.25rem 0.5rem 1.25rem;
}
.modal-footer {
  border-top: none;
  padding: 0.75rem 1.25rem 1.25rem 1.25rem;
  display: flex;
  justify-content: flex-end;
  gap: 0.5rem;
  background: transparent;
}
.form-control {
  border-radius: 8px;
  font-size: 0.97rem;
  padding: 0.5rem 0.9rem;
}
.btn-primary, .btn-secondary {
  border-radius: 30px;
  font-size: 0.97rem;
  padding: 0.45rem 1.5rem;
}
.btn-primary {
  background: linear-gradient(90deg, #AAAAAA 0%, #2c3e50 100%);
  border: none;
  font-weight: 600;
}
.btn-primary:hover {
  background: linear-gradient(90deg, #2c3e50 0%, #AAAAAA 100%);
}
@media (max-width: 480px) {
  .modal-dialog {
    max-width: 98vw;
    margin: 1rem auto;
  }
  .modal-content, .modal-header, .modal-body, .modal-footer {
    padding-left: 0.5rem !important;
    padding-right: 0.5rem !important;
  }
  .modal-title {
    font-size: 1rem;
  }
  .btn-primary, .btn-secondary {
    font-size: 0.93rem;
    padding: 0.4rem 1.1rem;
  }
}
/* Backdrop chuẩn Bootstrap */
.modal-backdrop {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background: rgba(30, 41, 59, 0.35) !important;
  z-index: 1050;
}
</style>