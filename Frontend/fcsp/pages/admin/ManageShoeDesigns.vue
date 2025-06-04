<template>
  <div class="admin-layout">
    <AdminSidebar />
    <div class="main-content">
      <div class="container-fluid py-4">
        <div class="row">
          <div class="col-12">
            <h1 class="mb-4 text-primary fw-bold">Quản lý thiết kế giày</h1>
            
            <!-- Search and Filter Section -->
            <div class="card mb-4 search-card">
              <div class="card-body">
                <div class="row g-3">
                  <div class="col-12 col-md-4">
                    <div class="input-group">
                      <span class="input-group-text bg-light">
                        <i class="bi bi-search"></i>
                      </span>
                      <input 
                        type="text" 
                        class="form-control" 
                        v-model="search" 
                        placeholder="Tìm kiếm thiết kế"
                      >
                      <button 
                        v-if="search" 
                        class="btn btn-outline-secondary" 
                        type="button"
                        @click="search = ''"
                      >
                        <i class="bi bi-x"></i>
                      </button>
                    </div>
                  </div>
                  <div class="col-12 col-md-4">
                    <div class="input-group">
                      <span class="input-group-text bg-light">
                        <i class="bi bi-funnel"></i>
                      </span>
                      <select class="form-select" v-model="categoryFilter">
                        <option value="">Tất cả danh mục</option>
                        <option v-for="category in categories" :key="category" :value="category">
                          {{ category }}
                        </option>
                      </select>
                    </div>
                  </div>
                  <div class="col-12 col-md-4">
                    <div class="input-group">
                      <span class="input-group-text bg-light">
                        <i class="bi bi-toggle-on"></i>
                      </span>
                      <select class="form-select" v-model="statusFilter">
                        <option value="">Tất cả trạng thái</option>
                        <option value="active">Đang hoạt động</option>
                        <option value="draft">Bản nháp</option>
                        <option value="archived">Đã lưu trữ</option>
                      </select>
                    </div>
                  </div>
                </div>
              </div>
            </div>
    
            <!-- View Toggle and Add Button -->
            <div class="d-flex justify-content-between align-items-center mb-4">
              <div class="btn-group" role="group">
                <button 
                  type="button" 
                  class="btn" 
                  :class="viewMode === 'grid' ? 'btn-primary' : 'btn-outline-primary'"
                  @click="viewMode = 'grid'"
                >
                  <i class="bi bi-grid-3x3-gap-fill me-1"></i> Lưới
                </button>
                <button 
                  type="button" 
                  class="btn" 
                  :class="viewMode === 'table' ? 'btn-primary' : 'btn-outline-primary'"
                  @click="viewMode = 'table'"
                >
                  <i class="bi bi-list-ul me-1"></i> Bảng
                </button>
              </div>
              <button class="btn btn-primary" @click="showAddDesignModal">
                <i class="bi bi-plus-circle me-1"></i> Thêm thiết kế mới
              </button>
            </div>
    
            <!-- Grid View -->
            <div v-if="viewMode === 'grid'" class="row g-3">
              <div v-if="loading" class="col-12 text-center py-5">
                <div class="spinner-border text-primary" role="status">
                  <span class="visually-hidden">Đang tải...</span>
                </div>
              </div>
              <div v-else-if="filteredDesigns.length === 0" class="col-12 text-center py-5">
                <div class="empty-state">
                  <i class="bi bi-search fs-1 text-muted"></i>
                  <p class="mt-3 mb-0">Không tìm thấy thiết kế nào</p>
                </div>
              </div>
              <div v-else class="col-12 col-sm-6 col-md-4 col-xl-3" v-for="design in filteredDesigns" :key="design.id">
                <div class="card design-card h-100">
                  <div class="design-status-badge" :class="getStatusClass(design.status)">
                    {{ getStatusText(design.status) }}
                  </div>
                  <div class="design-image-container">
                    <img :src="design.image" class="card-img-top design-image" :alt="design.name">
                    <div class="design-overlay">
                      <button class="btn btn-sm btn-light me-1" @click="viewDesignDetails(design)" title="Xem chi tiết">
                        <i class="bi bi-eye"></i>
                      </button>
                      <button class="btn btn-sm btn-light me-1" @click="editDesign(design)" title="Chỉnh sửa">
                        <i class="bi bi-pencil"></i>
                      </button>
                      <button class="btn btn-sm btn-light" @click="confirmDeleteDesign(design)" title="Xóa">
                        <i class="bi bi-trash"></i>
                      </button>
                    </div>
                  </div>
                  <div class="card-body">
                    <h5 class="card-title mb-1 text-truncate">{{ design.name }}</h5>
                    <p class="card-text text-muted small mb-2">{{ design.category }}</p>
                    <div class="d-flex justify-content-between align-items-center">
                      <span class="fw-bold text-primary">{{ formatCurrency(design.price) }}</span>
                      <span class="badge bg-light text-dark">SKU: {{ design.sku }}</span>
                    </div>
                  </div>
                </div>
              </div>
            </div>
    
            <!-- Table View -->
            <div v-if="viewMode === 'table'" class="card designs-card">
              <div class="card-body p-0">
                <div class="table-responsive">
                  <table class="table table-hover align-middle mb-0">
                    <thead class="table-light">
                      <tr>
                        <th>Hình ảnh</th>
                        <th>Tên thiết kế</th>
                        <th>SKU</th>
                        <th>Danh mục</th>
                        <th>Giá</th>
                        <th>Trạng thái</th>
                        <th>Ngày tạo</th>
                        <th class="text-end">Thao tác</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-if="loading">
                        <td colspan="8" class="text-center py-4">
                          <div class="spinner-border text-primary" role="status">
                            <span class="visually-hidden">Đang tải...</span>
                          </div>
                        </td>
                      </tr>
                      <tr v-else-if="filteredDesigns.length === 0">
                        <td colspan="8" class="text-center py-4">
                          Không tìm thấy thiết kế nào
                        </td>
                      </tr>
                      <tr v-else v-for="design in filteredDesigns" :key="design.id">
                        <td>
                          <div class="design-thumbnail">
                            <img :src="design.image" :alt="design.name" class="img-thumbnail">
                          </div>
                        </td>
                        <td class="fw-medium">{{ design.name }}</td>
                        <td>{{ design.sku }}</td>
                        <td>{{ design.category }}</td>
                        <td class="fw-medium text-primary">{{ formatCurrency(design.price) }}</td>
                        <td>
                          <span :class="['badge', getStatusBadgeClass(design.status)]">
                            {{ getStatusText(design.status) }}
                          </span>
                        </td>
                        <td class="date-text">{{ formatDate(design.createdAt) }}</td>
                        <td class="text-end">
                          <button 
                            class="btn btn-sm btn-outline-primary me-1" 
                            data-bs-toggle="tooltip" 
                            title="Xem chi tiết"
                            @click="viewDesignDetails(design)"
                          >
                            <i class="bi bi-eye"></i>
                          </button>
                          <button 
                            class="btn btn-sm btn-outline-success me-1" 
                            data-bs-toggle="tooltip" 
                            title="Chỉnh sửa"
                            @click="editDesign(design)"
                          >
                            <i class="bi bi-pencil"></i>
                          </button>
                          <button 
                            class="btn btn-sm btn-outline-danger" 
                            data-bs-toggle="tooltip" 
                            title="Xóa"
                            @click="confirmDeleteDesign(design)"
                          >
                            <i class="bi bi-trash"></i>
                          </button>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
              </div>
              <div class="card-footer bg-white d-flex justify-content-between align-items-center">
                <div>
                  Hiển thị <span class="fw-medium">{{ filteredDesigns.length }}</span> trên tổng số <span class="fw-medium">{{ designs.length }}</span> thiết kế
                </div>
              </div>
            </div>
    
            <!-- Design Details Modal -->
            <div class="modal fade" id="designDetailsModal" tabindex="-1" ref="designDetailsModal">
              <div class="modal-dialog modal-lg modal-dialog-scrollable">
                <div class="modal-content">
                  <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title">Chi tiết thiết kế</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                  </div>
                  <div class="modal-body" v-if="selectedDesign">
                    <div class="row g-4">
                      <div class="col-md-5">
                        <div class="design-detail-image-container">
                          <img :src="selectedDesign.image" class="img-fluid rounded" :alt="selectedDesign.name">
                          <span :class="['badge position-absolute top-0 end-0 m-2', getStatusBadgeClass(selectedDesign.status)]">
                            {{ getStatusText(selectedDesign.status) }}
                          </span>
                        </div>
                        <div class="row g-2 mt-2">
                          <div class="col-3" v-for="(image, index) in selectedDesign.additionalImages" :key="index">
                            <img :src="image" class="img-thumbnail design-thumbnail-small" alt="Additional view">
                          </div>
                        </div>
                      </div>
                      <div class="col-md-7">
                        <h3 class="mb-2">{{ selectedDesign.name }}</h3>
                        <p class="text-muted mb-3">SKU: {{ selectedDesign.sku }}</p>
                        
                        <div class="mb-3">
                          <h5 class="text-primary mb-2">Thông tin cơ bản</h5>
                          <div class="row g-2">
                            <div class="col-6">
                              <div class="detail-item">
                                <span class="detail-label">Danh mục:</span>
                                <span class="detail-value">{{ selectedDesign.category }}</span>
                              </div>
                            </div>
                            <div class="col-6">
                              <div class="detail-item">
                                <span class="detail-label">Giá:</span>
                                <span class="detail-value fw-bold text-primary">{{ formatCurrency(selectedDesign.price) }}</span>
                              </div>
                            </div>
                            <div class="col-6">
                              <div class="detail-item">
                                <span class="detail-label">Ngày tạo:</span>
                                <span class="detail-value">{{ formatDate(selectedDesign.createdAt) }}</span>
                              </div>
                            </div>
                            <div class="col-6">
                              <div class="detail-item">
                                <span class="detail-label">Cập nhật lần cuối:</span>
                                <span class="detail-value">{{ formatDate(selectedDesign.updatedAt) }}</span>
                              </div>
                            </div>
                          </div>
                        </div>
                        
                        <div class="mb-3">
                          <h5 class="text-primary mb-2">Thông số kỹ thuật</h5>
                          <div class="row g-2">
                            <div class="col-6">
                              <div class="detail-item">
                                <span class="detail-label">Chất liệu:</span>
                                <span class="detail-value">{{ selectedDesign.material }}</span>
                              </div>
                            </div>
                            <div class="col-6">
                              <div class="detail-item">
                                <span class="detail-label">Màu sắc:</span>
                                <span class="detail-value">{{ selectedDesign.colors.join(', ') }}</span>
                              </div>
                            </div>
                            <div class="col-6">
                              <div class="detail-item">
                                <span class="detail-label">Kích cỡ:</span>
                                <span class="detail-value">{{ selectedDesign.sizes.join(', ') }}</span>
                              </div>
                            </div>
                            <div class="col-6">
                              <div class="detail-item">
                                <span class="detail-label">Giới tính:</span>
                                <span class="detail-value">{{ selectedDesign.gender }}</span>
                              </div>
                            </div>
                          </div>
                        </div>
                        
                        <div class="mb-3">
                          <h5 class="text-primary mb-2">Mô tả</h5>
                          <p>{{ selectedDesign.description }}</p>
                        </div>
                        
                        <div class="mb-3">
                          <h5 class="text-primary mb-2">Đặc điểm nổi bật</h5>
                          <ul class="features-list">
                            <li v-for="(feature, index) in selectedDesign.features" :key="index">
                              {{ feature }}
                            </li>
                          </ul>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-primary" @click="editDesign(selectedDesign)">Edit</button>
                  </div>
                </div>
              </div>
            </div>
    
            <!-- Edit/Add Design Modal -->
            <div class="modal fade" id="editDesignModal" tabindex="-1" ref="editDesignModal">
              <div class="modal-dialog modal-lg modal-dialog-scrollable">
                <div class="modal-content">
                  <div class="modal-header" :class="isNewDesign ? 'bg-success text-white' : 'bg-primary text-white'">
                    <h5 class="modal-title">{{ isNewDesign ? 'Thêm thiết kế mới' : 'Chỉnh sửa thiết kế' }}</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                  </div>
                  <div class="modal-body">
                    <form @submit.prevent="saveDesign" id="designForm">
                      <div class="row g-3">
                        <div class="col-md-6">
                          <div class="mb-3">
                            <label for="designName" class="form-label">Design name</label>
                            <input type="text" class="form-control" id="designName" v-model="editedDesign.name" required>
                          </div>
                        </div>
                        <div class="col-md-6">
                          <div class="mb-3">
                            <label for="designSku" class="form-label">SKU</label>
                            <input type="text" class="form-control" id="designSku" v-model="editedDesign.sku" required>
                          </div>
                        </div>
                        <div class="col-md-6">
                          <div class="mb-3">
                            <label for="designCategory" class="form-label">Category</label>
                            <select class="form-select" id="designCategory" v-model="editedDesign.category" required>
                              <option v-for="category in categories" :key="category" :value="category">
                                {{ category }}
                              </option>
                            </select>
                          </div>
                        </div>
                        <div class="col-md-6">
                          <div class="mb-3">
                            <label for="designPrice" class="form-label">Price</label>
                            <div class="input-group">
                              <span class="input-group-text">₫</span>
                              <input type="number" class="form-control" id="designPrice" v-model="editedDesign.price" required min="0">
                            </div>
                          </div>
                        </div>
                        <div class="col-md-6">
                          <div class="mb-3">
                            <label for="designMaterial" class="form-label">Material</label>
                            <input type="text" class="form-control" id="designMaterial" v-model="editedDesign.material">
                          </div>
                        </div>
                        <div class="col-md-6">
                          <div class="mb-3">
                            <label for="designGender" class="form-label">Gender</label>
                            <select class="form-select" id="designGender" v-model="editedDesign.gender">
                              <option value="Nam">Male</option>
                              <option value="Nữ">Female</option>
                              <option value="Unisex">Unisex</option>
                            </select>
                          </div>
                        </div>
                        <div class="col-md-6">
                          <div class="mb-3">
                            <label class="form-label">Color</label>
                            <div class="color-checkboxes bg-light p-2 rounded">
                              <div class="row g-2">
                                <div class="col-4" v-for="color in availableColors" :key="color">
                                  <div class="form-check">
                                    <input 
                                      class="form-check-input" 
                                      type="checkbox" 
                                      :id="'color-' + color"
                                      :value="color"
                                      v-model="editedDesign.colors"
                                    >
                                    <label class="form-check-label" :for="'color-' + color">
                                      {{ color }}
                                    </label>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                        <div class="col-md-6">
                          <div class="mb-3">
                            <label class="form-label">Size</label>
                            <div class="size-checkboxes bg-light p-2 rounded">
                              <div class="row g-2">
                                <div class="col-3" v-for="size in availableSizes" :key="size">
                                  <div class="form-check">
                                    <input 
                                      class="form-check-input" 
                                      type="checkbox" 
                                      :id="'size-' + size"
                                      :value="size"
                                      v-model="editedDesign.sizes"
                                    >
                                    <label class="form-check-label" :for="'size-' + size">
                                      {{ size }}
                                    </label>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                        <div class="col-12">
                          <div class="mb-3">
                            <label for="designDescription" class="form-label">Description</label>
                            <textarea class="form-control" id="designDescription" rows="3" v-model="editedDesign.description"></textarea>
                          </div>
                        </div>
                        <div class="col-12">
                          <div class="mb-3">
                            <label for="designFeatures" class="form-label">Outstanding features (each line is a feature)</label>
                            <textarea class="form-control" id="designFeatures" rows="3" v-model="featuresText"></textarea>
                          </div>
                        </div>
                        <div class="col-md-6">
                          <div class="mb-3">
                            <label for="designImage" class="form-label">Main image</label>
                            <input type="text" class="form-control" id="designImage" v-model="editedDesign.image" placeholder="URL hình ảnh">
                            <div class="mt-2" v-if="editedDesign.image">
                              <img :src="editedDesign.image" class="img-thumbnail" style="height: 100px;" alt="Main image preview">
                            </div>
                          </div>
                        </div>
                        <div class="col-md-6">
                          <div class="mb-3">
                            <label for="designStatus" class="form-label">Status</label>
                            <select class="form-select" id="designStatus" v-model="editedDesign.status" required>
                              <option value="active">Active</option>
                              <option value="draft">Draft</option>
                              <option value="archived">Archived</option>
                            </select>
                          </div>
                        </div>
                        <div class="col-12">
                          <div class="mb-3">
                            <label class="form-label">Additional Images</label>
                            <div class="input-group mb-2" v-for="(image, index) in editedDesign.additionalImages" :key="index">
                              <input type="text" class="form-control" v-model="editedDesign.additionalImages[index]" placeholder="URL hình ảnh">
                              <button class="btn btn-outline-danger" type="button" @click="removeAdditionalImage(index)">
                                <i class="bi bi-trash"></i>
                              </button>
                            </div>
                            <button type="button" class="btn btn-outline-primary btn-sm" @click="addAdditionalImage">
                              <i class="bi bi-plus-circle me-1"></i> Add image
                            </button>
                          </div>
                        </div>
                      </div>
                    </form>
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <button type="submit" form="designForm" class="btn" :class="isNewDesign ? 'btn-success' : 'btn-primary'">Save</button>
                  </div>
                </div>
              </div>
            </div>
    
            <!-- Delete Confirmation Modal -->
            <div class="modal fade" id="deleteDesignModal" tabindex="-1" ref="deleteDesignModal">
              <div class="modal-dialog">
                <div class="modal-content">
                  <div class="modal-header bg-danger text-white">
                    <h5 class="modal-title">Confirm deletion</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                  </div>
                  <div class="modal-body" v-if="selectedDesign">
                    <p>Are you sure you want to delete the design? <strong>{{ selectedDesign.name }}</strong>?</p>
                    <p class="text-danger">
                      <i class="bi bi-exclamation-triangle-fill me-2"></i>
                      This action cannot be undone.
                    </p>
                  </div>
                  <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Hủy</button>
                    <button type="button" class="btn btn-danger" @click="deleteDesign">Xóa</button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import AdminSidebar from '@/components/AdminSidebar.vue';

export default {
  name: 'ShoeDesignManagement',
  components: {
    AdminSidebar
  },
  data() {
    return {
      search: '',
      categoryFilter: '',
      statusFilter: '',
      viewMode: 'grid',
      loading: false,
      selectedDesign: null,
      editedDesign: {},
      isNewDesign: false,
      featuresText: '',
      designDetailsModal: null,
      editDesignModal: null,
      deleteDesignModal: null,
      categories: [
        'Giày thể thao',
        'Giày chạy bộ',
        'Giày bóng rổ',
        'Giày đá banh',
        'Giày thời trang',
        'Giày sandal',
        'Giày cao gót'
      ],
      availableColors: [
        'Đen', 'Trắng', 'Đỏ', 'Xanh dương', 'Xanh lá', 'Vàng', 
        'Cam', 'Tím', 'Hồng', 'Xám', 'Nâu', 'Bạc'
      ],
      availableSizes: [
        '35', '36', '37', '38', '39', '40', '41', '42', '43', '44', '45', '46'
      ],
      designs: [
        {
          id: 'DSN001',
          name: 'Nike Air Max 270',
          sku: 'NKE-AM270-BLK',
          category: 'Giày thể thao',
          price: 3200000,
          status: 'active',
          createdAt: '2024-01-15',
          updatedAt: '2024-03-10',
          material: 'Vải lưới, cao su',
          colors: ['Đen', 'Trắng', 'Đỏ'],
          sizes: ['38', '39', '40', '41', '42', '43'],
          gender: 'Unisex',
          description: 'Nike Air Max 270 là mẫu giày thể thao với đệm khí Air Max lớn nhất từ trước đến nay của Nike, mang lại cảm giác thoải mái suốt cả ngày.',
          features: [
            'Đệm khí Air Max 270 độc đáo',
            'Phần trên bằng vải lưới thoáng khí',
            'Đế giữa bằng foam nhẹ',
            'Đế ngoài bằng cao su bền bỉ'
          ],
          image: 'https://via.placeholder.com/500x500.png?text=Nike+Air+Max+270',
          additionalImages: [
            'https://via.placeholder.com/500x500.png?text=Nike+Air+Max+270+Side',
            'https://via.placeholder.com/500x500.png?text=Nike+Air+Max+270+Back',
            'https://via.placeholder.com/500x500.png?text=Nike+Air+Max+270+Top'
          ]
        },
        {
          id: 'DSN002',
          name: 'Adidas Ultra Boost',
          sku: 'ADS-UB21-WHT',
          category: 'Giày chạy bộ',
          price: 4500000,
          status: 'active',
          createdAt: '2024-01-20',
          updatedAt: '2024-03-15',
          material: 'Primeknit, Boost',
          colors: ['Trắng', 'Đen', 'Xanh dương'],
          sizes: ['39', '40', '41', '42', '43', '44'],
          gender: 'Nam',
          description: 'Adidas Ultra Boost là mẫu giày chạy bộ cao cấp với công nghệ đệm Boost mang lại khả năng đàn hồi và thoải mái tuyệt vời.',
          features: [
            'Công nghệ đệm Boost độc quyền',
            'Thân giày Primeknit co giãn',
            'Hệ thống Torsion System hỗ trợ giữa bàn chân',
            'Đế ngoài Continental™ bám tốt trên mọi bề mặt'
          ],
          image: 'https://via.placeholder.com/500x500.png?text=Adidas+Ultra+Boost',
          additionalImages: [
            'https://via.placeholder.com/500x500.png?text=Adidas+Ultra+Boost+Side',
            'https://via.placeholder.com/500x500.png?text=Adidas+Ultra+Boost+Back',
            'https://via.placeholder.com/500x500.png?text=Adidas+Ultra+Boost+Top'
          ]
        },
        {
          id: 'DSN003',
          name: 'Puma RS-X',
          sku: 'PMA-RSX-BLU',
          category: 'Giày thời trang',
          price: 2800000,
          status: 'active',
          createdAt: '2024-02-05',
          updatedAt: '2024-03-20',
          material: 'Da tổng hợp, lưới',
          colors: ['Xanh dương', 'Trắng', 'Đen'],
          sizes: ['38', '39', '40', '41', '42'],
          gender: 'Unisex',
          description: 'Puma RS-X là mẫu giày thời trang với thiết kế chunky retro đang rất được ưa chuộng, kết hợp giữa phong cách thể thao và thời trang đường phố.',
          features: [
            'Thiết kế chunky retro đặc trưng',
            'Công nghệ Running System (RS) cải tiến',
            'Đế giữa PU đúc phun',
            'Phần trên kết hợp nhiều chất liệu'
          ],
          image: 'https://via.placeholder.com/500x500.png?text=Puma+RS-X',
          additionalImages: [
            'https://via.placeholder.com/500x500.png?text=Puma+RS-X+Side',
            'https://via.placeholder.com/500x500.png?text=Puma+RS-X+Back',
            'https://via.placeholder.com/500x500.png?text=Puma+RS-X+Top'
          ]
        },
        {
          id: 'DSN004',
          name: 'Jordan 1 Retro High',
          sku: 'JRD-1RH-RED',
          category: 'Giày bóng rổ',
          price: 5500000,
          status: 'draft',
          createdAt: '2024-02-15',
          updatedAt: '2024-03-25',
          material: 'Da thật, cao su',
          colors: ['Đỏ', 'Đen', 'Trắng'],
          sizes: ['40', '41', '42', '43', '44', '45'],
          gender: 'Nam',
          description: 'Jordan 1 Retro High là mẫu giày bóng rổ huyền thoại với thiết kế kinh điển, là biểu tượng của văn hóa sneaker toàn cầu.',
          features: [
            'Chất liệu da cao cấp',
            'Đệm Air-Sole ở gót chân',
            'Cổ giày cao cổ điển',
            'Logo Wings và Swoosh đặc trưng'
          ],
          image: 'https://via.placeholder.com/500x500.png?text=Jordan+1+Retro+High',
          additionalImages: [
            'https://via.placeholder.com/500x500.png?text=Jordan+1+Retro+High+Side',
            'https://via.placeholder.com/500x500.png?text=Jordan+1+Retro+High+Back',
            'https://via.placeholder.com/500x500.png?text=Jordan+1+Retro+High+Top'
          ]
        },
        {
          id: 'DSN005',
          name: 'Converse Chuck 70',
          sku: 'CNV-C70-BLK',
          category: 'Giày thời trang',
          price: 1800000,
          status: 'active',
          createdAt: '2024-02-20',
          updatedAt: '2024-03-28',
          material: 'Vải canvas, cao su',
          colors: ['Đen', 'Trắng', 'Đỏ', 'Xanh dương'],
          sizes: ['36', '37', '38', '39', '40', '41', '42', '43'],
          gender: 'Unisex',
          description: 'Converse Chuck 70 là phiên bản nâng cấp của dòng giày Chuck Taylor All Star kinh điển, với chất liệu cao cấp hơn và sự thoải mái được cải thiện.',
          features: [
            'Vải canvas dày dặn hơn',
            'Đệm lót OrthoLite cải tiến',
            'Đường may tỉ mỉ hơn',
            'Đế giày cao hơn với sọc màu kem đặc trưng'
          ],
          image: 'https://via.placeholder.com/500x500.png?text=Converse+Chuck+70',
          additionalImages: [
            'https://via.placeholder.com/500x500.png?text=Converse+Chuck+70+Side',
            'https://via.placeholder.com/500x500.png?text=Converse+Chuck+70+Back',
            'https://via.placeholder.com/500x500.png?text=Converse+Chuck+70+Top'
          ]
        },
        {
          id: 'DSN006',
          name: 'Vans Old Skool',
          sku: 'VNS-OS-BLK',
          category: 'Giày thời trang',
          price: 1600000,
          status: 'active',
          createdAt: '2024-03-01',
          updatedAt: '2024-03-30',
          material: 'Da lộn, vải canvas',
          colors: ['Đen', 'Xanh dương', 'Đỏ'],
          sizes: ['36', '37', '38', '39', '40', '41', '42', '43'],
          gender: 'Unisex',
          description: 'Vans Old Skool là mẫu giày skate kinh điển với thiết kế sọc bên hông đặc trưng, là biểu tượng của văn hóa đường phố và skateboard.',
          features: [
            'Sọc Jazz Stripe đặc trưng',
            'Mũi giày được gia cố',
            'Đế waffle đặc trưng của Vans',
            'Cổ giày có đệm'
          ],
          image: 'https://via.placeholder.com/500x500.png?text=Vans+Old+Skool',
          additionalImages: [
            'https://via.placeholder.com/500x500.png?text=Vans+Old+Skool+Side',
            'https://via.placeholder.com/500x500.png?text=Vans+Old+Skool+Back',
            'https://via.placeholder.com/500x500.png?text=Vans+Old+Skool+Top'
          ]
        },
        {
          id: 'DSN007',
          name: 'New Balance 574',
          sku: 'NB-574-GRY',
          category: 'Giày thể thao',
          price: 2200000,
          status: 'archived',
          createdAt: '2024-03-05',
          updatedAt: '2024-03-31',
          material: 'Da lộn, lưới',
          colors: ['Xám', 'Xanh dương', 'Đen'],
          sizes: ['39', '40', '41', '42', '43', '44'],
          gender: 'Nam',
          description: 'New Balance 574 là mẫu giày thể thao cổ điển với thiết kế retro và sự thoải mái đặc trưng của thương hiệu New Balance.',
          features: [
            'Công nghệ đệm ENCAP',
            'Phần trên kết hợp da lộn và lưới',
            'Đế giữa EVA nhẹ',
            'Logo N đặc trưng'
          ],
          image: 'https://via.placeholder.com/500x500.png?text=New+Balance+574',
          additionalImages: [
            'https://via.placeholder.com/500x500.png?text=New+Balance+574+Side',
            'https://via.placeholder.com/500x500.png?text=New+Balance+574+Back',
            'https://via.placeholder.com/500x500.png?text=New+Balance+574+Top'
          ]
        },
        {
          id: 'DSN008',
          name: 'Asics Gel-Kayano',
          sku: 'ASC-GK28-BLU',
          category: 'Giày chạy bộ',
          price: 3800000,
          status: 'draft',
          createdAt: '2024-03-10',
          updatedAt: '2024-04-01',
          material: 'Vải lưới, nhựa tổng hợp',
          colors: ['Xanh dương', 'Đen', 'Xám'],
          sizes: ['39', '40', '41', '42', '43', '44'],
          gender: 'Nam',
          description: 'Asics Gel-Kayano là mẫu giày chạy bộ cao cấp với công nghệ đệm GEL và hỗ trợ ổn định, lý tưởng cho các runner có bàn chân lệch trong.',
          features: [
            'Công nghệ đệm GEL ở gót và bàn chân trước',
            'Hệ thống Dynamic DuoMax hỗ trợ ổn định',
            'Công nghệ FlyteFoam nhẹ và đàn hồi',
            'Công nghệ AHAR+ ở đế ngoài bền bỉ'
          ],
          image: 'https://via.placeholder.com/500x500.png?text=Asics+Gel-Kayano',
          additionalImages: [
            'https://via.placeholder.com/500x500.png?text=Asics+Gel-Kayano+Side',
            'https://via.placeholder.com/500x500.png?text=Asics+Gel-Kayano+Back',
            'https://via.placeholder.com/500x500.png?text=Asics+Gel-Kayano+Top'
          ]
        }
      ]
    }
  },
  computed: {
    filteredDesigns() {
      let result = [...this.designs];
      
      // Apply category filter
      if (this.categoryFilter) {
        result = result.filter(design => design.category === this.categoryFilter);
      }
      
      // Apply status filter
      if (this.statusFilter) {
        result = result.filter(design => design.status === this.statusFilter);
      }
      
      // Apply search filter
      if (this.search) {
        const searchLower = this.search.toLowerCase();
        result = result.filter(design => 
          design.name.toLowerCase().includes(searchLower) ||
          design.sku.toLowerCase().includes(searchLower) ||
          design.category.toLowerCase().includes(searchLower) ||
          design.description.toLowerCase().includes(searchLower)
        );
      }
      
      return result;
    }
  },
  methods: {
    formatCurrency(value) {
      return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
      }).format(value);
    },
    formatDate(date) {
      if (!date) return '';
      return new Date(date).toLocaleDateString('vi-VN');
    },
    getStatusText(status) {
      const statusMap = {
        'active': 'Đang hoạt động',
        'draft': 'Bản nháp',
        'archived': 'Đã lưu trữ'
      };
      return statusMap[status] || status;
    },
    getStatusClass(status) {
      const classes = {
        'active': 'status-active',
        'draft': 'status-draft',
        'archived': 'status-archived'
      };
      return classes[status] || '';
    },
    getStatusBadgeClass(status) {
      const classes = {
        'active': 'bg-success',
        'draft': 'bg-warning text-dark',
        'archived': 'bg-secondary'
      };
      return classes[status] || 'bg-secondary';
    },
    viewDesignDetails(design) {
      this.selectedDesign = design;
      this.designDetailsModal.show();
    },
    editDesign(design) {
      this.isNewDesign = false;
      this.editedDesign = JSON.parse(JSON.stringify(design)); // Deep copy
      this.featuresText = design.features.join('\n');
      
      // Close details modal if open
      if (this.designDetailsModal && this.designDetailsModal._isShown) {
        this.designDetailsModal.hide();
      }
      
      this.editDesignModal.show();
    },
    showAddDesignModal() {
      this.isNewDesign = true;
      this.editedDesign = {
        id: 'DSN' + (this.designs.length + 1).toString().padStart(3, '0'),
        name: '',
        sku: '',
        category: this.categories[0],
        price: 0,
        status: 'draft',
        createdAt: new Date().toISOString().split('T')[0],
        updatedAt: new Date().toISOString().split('T')[0],
        material: '',
        colors: [],
        sizes: [],
        gender: 'Unisex',
        description: '',
        features: [],
        image: '',
        additionalImages: []
      };
      this.featuresText = '';
      this.editDesignModal.show();
    },
    confirmDeleteDesign(design) {
      this.selectedDesign = design;
      this.deleteDesignModal.show();
    },
    async deleteDesign() {
      try {
        // Remove the design from the array
        const index = this.designs.findIndex(d => d.id === this.selectedDesign.id);
        if (index !== -1) {
          this.designs.splice(index, 1);
        }
        
        // Close the modal
        this.deleteDesignModal.hide();
        
        // Show success message
        this.showToast(`Thiết kế "${this.selectedDesign.name}" đã được xóa thành công`, 'success');
      } catch (error) {
        this.showToast('Có lỗi xảy ra khi xóa thiết kế', 'danger');
      }
    },
    async saveDesign() {
      try {
        // Convert features text to array
        this.editedDesign.features = this.featuresText
          .split('\n')
          .map(line => line.trim())
          .filter(line => line.length > 0);
        
        // Update the updatedAt date
        this.editedDesign.updatedAt = new Date().toISOString().split('T')[0];
        
        if (this.isNewDesign) {
          // Add new design
          this.designs.push(this.editedDesign);
          this.showToast('Thiết kế mới đã được tạo thành công', 'success');
        } else {
          // Update existing design
          const index = this.designs.findIndex(d => d.id === this.editedDesign.id);
          if (index !== -1) {
            this.designs[index] = { ...this.editedDesign };
          }
          this.showToast('Cập nhật thiết kế thành công', 'success');
        }
        
        // Close the modal
        this.editDesignModal.hide();
      } catch (error) {
        this.showToast('Có lỗi xảy ra khi lưu thiết kế', 'danger');
      }
    },
    addAdditionalImage() {
      this.editedDesign.additionalImages.push('');
    },
    removeAdditionalImage(index) {
      this.editedDesign.additionalImages.splice(index, 1);
    },
    showToast(message, type = 'success') {
      // Create a Bootstrap toast programmatically
      const toastContainer = document.getElementById('toast-container') || this.createToastContainer();
      
      const toastEl = document.createElement('div');
      toastEl.className = `toast align-items-center text-white bg-${type} border-0`;
      toastEl.setAttribute('role', 'alert');
      toastEl.setAttribute('aria-live', 'assertive');
      toastEl.setAttribute('aria-atomic', 'true');
      
      toastEl.innerHTML = `
        <div class="d-flex">
          <div class="toast-body">
            ${message}
          </div>
          <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
        </div>
      `;
      
      toastContainer.appendChild(toastEl);
      
      // Initialize and show the toast
      const toast = new bootstrap.Toast(toastEl, { autohide: true, delay: 3000 });
      toast.show();
      
      // Remove the toast element after it's hidden
      toastEl.addEventListener('hidden.bs.toast', () => {
        toastEl.remove();
      });
    },
    createToastContainer() {
      const container = document.createElement('div');
      container.id = 'toast-container';
      container.className = 'toast-container position-fixed bottom-0 end-0 p-3';
      container.style.zIndex = '1050';
      document.body.appendChild(container);
      return container;
    },
    async fetchDesigns() {
      this.loading = true;
      try {
        // TODO: Implement API call to fetch designs
        // Using mock data for demonstration
        await new Promise(resolve => setTimeout(resolve, 500)); // Simulate API delay
        // Designs are already set in data()
      } catch (error) {
        this.showToast('Có lỗi xảy ra khi tải danh sách thiết kế', 'danger');
      } finally {
        this.loading = false;
      }
    },
  },
  mounted() {
    
  },
  created() {
    this.fetchDesigns();
  }
}
</script>

<style scoped>
.admin-layout {
  display: flex;
  min-height: 100vh;
}

.main-content {
  flex: 1;
  margin-left: 250px;
  padding: 20px;
}

/* Custom styling */
.search-card {
  border: none;
  box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
  transition: all 0.3s ease;
}

.search-card:hover {
  box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.1);
}

.designs-card {
  border: none;
  box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
  overflow: hidden;
}

/* Design card styling */
.design-card {
  border: none;
  box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
}

.design-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
}

.design-image-container {
  position: relative;
  overflow: hidden;
  height: 200px;
}

.design-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
  transition: transform 0.3s ease;
}

.design-card:hover .design-image {
  transform: scale(1.05);
}

.design-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  opacity: 0;
  transition: opacity 0.3s ease;
}

.design-card:hover .design-overlay {
  opacity: 1;
}

.design-status-badge {
  position: absolute;
  top: 10px;
  right: 10px;
  padding: 0.25rem 0.5rem;
  border-radius: 0.25rem;
  font-size: 0.75rem;
  font-weight: 500;
  z-index: 1;
  text-transform: uppercase;
}

.status-active {
  background-color: rgba(25, 135, 84, 0.9);
  color: white;
}

.status-draft {
  background-color: rgba(255, 193, 7, 0.9);
  color: #212529;
}

.status-archived {
  background-color: rgba(108, 117, 125, 0.9);
  color: white;
}

/* Table styling */
.table th {
  font-weight: 600;
  text-transform: uppercase;
  font-size: 0.8rem;
  letter-spacing: 0.5px;
}

.table td {
  vertical-align: middle;
  padding: 0.75rem 1rem;
}

.design-thumbnail {
  width: 60px;
  height: 60px;
  overflow: hidden;
}

.design-thumbnail img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.design-thumbnail-small {
  width: 100%;
  height: 60px;
  object-fit: cover;
}

.date-text {
  color: #6c757d;
  font-size: 0.9rem;
}

/* Status badge styling */
.badge {
  padding: 0.5em 0.75em;
  font-weight: 500;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  font-size: 0.75rem;
}

/* Modal styling */
.modal-header {
  border-bottom: 0;
}

.modal-footer {
  border-top: 0;
}

.design-detail-image-container {
  position: relative;
}

/* Detail styling */
.detail-item {
  margin-bottom: 0.5rem;
}

.detail-label {
  font-weight: 600;
  color: #6c757d;
}

.detail-value {
  margin-left: 0.5rem;
}

.features-list {
  padding-left: 1.5rem;
}

.features-list li {
  margin-bottom: 0.5rem;
}

/* Empty state */
.empty-state {
  padding: 2rem;
  text-align: center;
  color: #6c757d;
}

/* Button styling */
.btn-sm {
  padding: 0.25rem 0.5rem;
  font-size: 0.8rem;
}

.btn-outline-primary, .btn-outline-success, .btn-outline-danger {
  transition: all 0.2s ease;
}

.btn-outline-primary:hover, .btn-outline-success:hover, .btn-outline-danger:hover {
  transform: translateY(-2px);
  box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
}

/* Responsive improvements */
@media (max-width: 768px) {
  .card-body {
    padding: 1rem;
  }

  .table th, .table td {
    padding: 0.5rem;
  }

  .btn-sm {
    padding: 0.2rem 0.4rem;
  }
  
  .design-image-container {
    height: 150px;
  }
}
</style>