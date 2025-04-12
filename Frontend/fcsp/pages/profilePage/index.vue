<template>
  <div class="container py-4">
    <div class="row">
      <div class="col-12">
        <h2 class="mb-0">Hồ Sơ Của Tôi</h2>
        <p class="text-muted">Quản lý thông tin hồ sơ để bảo mật tài khoản</p>
        <hr class="my-3" />
      </div>
    </div>

    <div class="row">
      <div class="col-md-8">
        <form @submit.prevent="saveProfile">
          <div class="mb-3 row">
            <label class="col-sm-3 col-form-label text-md-end">Tên đăng nhập</label>
            <div class="col-sm-9">
              <span class="form-control-plaintext text-primary">biducbi</span>
            </div>
          </div>

          <div class="mb-3 row">
            <label for="fullName" class="col-sm-3 col-form-label text-md-end">Tên</label>
            <div class="col-sm-9">
              <input
                type="text"
                class="form-control"
                id="fullName"
                v-model="profile.fullName"
                placeholder="Nhập tên của bạn"
              />
            </div>
          </div>

          <div class="mb-3 row">
            <label class="col-sm-3 col-form-label text-md-end">Email</label>
            <div class="col-sm-9 d-flex align-items-center">
              <span class="me-2">ho************@icloud.com</span>
              <a href="#" class="text-primary">Thay Đổi</a>
            </div>
          </div>

          <div class="mb-3 row">
            <label class="col-sm-3 col-form-label text-md-end">Số điện thoại</label>
            <div class="col-sm-9 d-flex align-items-center">
              <span class="me-2">*********74</span>
              <a href="#" class="text-primary">Thay Đổi</a>
            </div>
          </div>

          <div class="mb-3 row">
            <label class="col-sm-3 col-form-label text-md-end">Giới tính</label>
            <div class="col-sm-9">
              <div class="form-check form-check-inline">
                <input
                  class="form-check-input"
                  type="radio"
                  name="gender"
                  id="male"
                  value="Nam"
                  v-model="profile.gender"
                />
                <label class="form-check-label" for="male">Nam</label>
              </div>
              <div class="form-check form-check-inline">
                <input
                  class="form-check-input"
                  type="radio"
                  name="gender"
                  id="female"
                  value="Nữ"
                  v-model="profile.gender"
                />
                <label class="form-check-label" for="female">Nữ</label>
              </div>
              <div class="form-check form-check-inline">
                <input
                  class="form-check-input"
                  type="radio"
                  name="gender"
                  id="other"
                  value="Khác"
                  v-model="profile.gender"
                />
                <label class="form-check-label" for="other">Khác</label>
              </div>
            </div>
          </div>

          <div class="mb-3 row">
            <label class="col-sm-3 col-form-label text-md-end">Ngày sinh</label>
            <div class="col-sm-9 d-flex align-items-center">
              <span class="me-2">**/*/2002</span>
              <a href="#" class="text-primary">Thay Đổi</a>
            </div>
          </div>

          <div class="mb-3 row">
            <div class="col-sm-9 offset-sm-3">
              <button type="submit" class="btn btn-danger px-4">Lưu</button>
            </div>
          </div>
        </form>
      </div>

      <div class="col-md-4 text-center">
        <div class="d-flex flex-column align-items-center">
          <div class="position-relative mb-3">
            <img
              :src="profile.avatar || 'https://hebbkx1anhila5yf.public.blob.vercel-storage.com/image-Pg8Xar0cs5YJAv7GozxRFA8TXVJ7tP.png'"
              alt="Profile Picture"
              class="rounded-circle img-thumbnail"
              style="width: 150px; height: 150px; object-fit: cover"
            />
          </div>
          <button class="btn btn-outline-secondary mb-2" @click="selectImage">Chọn Ảnh</button>
          <input
            type="file"
            ref="fileInput"
            @change="onFileChange"
            accept=".jpeg,.jpg,.png"
            style="display: none"
          />
          <p class="text-muted small mb-1">Dung lượng file tối đa 1 MB</p>
          <p class="text-muted small">Định dạng:.JPEG, .PNG</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'ProfilePage',
  data() {
    return {
      profile: {
        username: 'biducbi',
        fullName: 'Hoàng Đình Đức',
        email: 'ho************@icloud.com',
        phone: '*********74',
        gender: 'Nam',
        birthdate: '**/*/2002',
        avatar: null
      }
    }
  },
  methods: {
    saveProfile() {
      // Here you would typically send the profile data to your backend
      alert('Thông tin hồ sơ đã được lưu!')
    },
    selectImage() {
      this.$refs.fileInput.click()
    },
    onFileChange(event) {
      const file = event.target.files[0]
      if (!file) return
      
      // Check file size (1MB = 1048576 bytes)
      if (file.size > 1048576) {
        alert('Kích thước file quá lớn. Vui lòng chọn file nhỏ hơn 1MB.')
        return
      }
      
      // Check file type
      if (!['image/jpeg', 'image/png'].includes(file.type)) {
        alert('Chỉ chấp nhận file JPEG hoặc PNG.')
        return
      }
      
      // Create a preview
      const reader = new FileReader()
      reader.onload = (e) => {
        this.profile.avatar = e.target.result
      }
      reader.readAsDataURL(file)
    }
  }
}
</script>

<style>
/* You can add custom styles here if needed */
</style>