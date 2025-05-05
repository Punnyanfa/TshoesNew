<template>
    <div class="custom-detail-page">
      <!-- Thông tin sản phẩm ở góc trái trên -->
      <div class="product-info">
        <div class="product-name-container">
          <h2 class="product-name">{{ customProductName }}</h2>
          <button class="edit-name-btn" @click="openEditNameModal">
            <i class="fas fa-edit"></i> Sửa tên
          </button>
        </div>
        <p class="product-price">{{ formatPrice(basePrice) }}</p>
        <p class="product-surcharge" v-if="surcharge > 0">Phụ phí: {{ formatPrice(surcharge) }}</p>
        
        <!-- Dropdown chọn nhà sản xuất -->
        <div class="manufacturer-selector">
          <label for="manufacturer">Thương hiệu:</label>
          <select id="manufacturer" v-model="selectedManufacturer" @change="handleManufacturerChange">
            <option v-for="(mfr, index) in manufacturers" :key="index" :value="mfr.id">
              {{ mfr.name }}
            </option>
          </select>
        </div>
      </div>
      
      <!-- Các nút chức năng ở góc phải trên -->
      <div class="action-buttons" style="display: flex; gap: 10px; justify-content: flex-end;">
        <button class="action-button" @click="showSurchargeInfo = true">Thông tin phụ phí</button>
        <button class="action-button" @click="openCaptureModal">Tải ảnh</button>
        <button class="action-button primary-button" @click="handleDone">Hoàn thành</button>
      </div>
      
      <!-- Modal hiển thị thông tin phụ phí -->
      <div v-if="showSurchargeInfo" class="surcharge-modal">
        <div class="surcharge-modal-content">
          <div class="surcharge-modal-header">
            <h3>Thông tin phụ phí {{ currentManufacturer.name }}</h3>
            <button class="close-button" @click="showSurchargeInfo = false">×</button>
          </div>
          <div class="surcharge-modal-body">
            <h4>Bảng phụ phí tùy chỉnh</h4>
            <div class="component-surcharge-details">
              <div class="surcharge-detail-table">
                <div class="surcharge-detail-header">
                  <div class="detail-col">Thành phần</div>
                  <div class="detail-col">Phụ phí màu sắc</div>
                  <div class="detail-col">Phụ phí hình ảnh</div>
                </div>
                <div v-for="comp in components" :key="comp.value" class="surcharge-detail-row">
                  <div class="detail-col">{{ comp.name }}</div>
                  <div class="detail-col">{{ formatPrice(currentManufacturer.surcharges.colorChange * currentManufacturer.surcharges.componentRates[comp.value]) }}</div>
                  <div class="detail-col">{{ formatPrice(currentManufacturer.surcharges.imageApplication * currentManufacturer.surcharges.componentRates[comp.value]) }}</div>
                </div>
              </div>
            </div>
            
            <div class="surcharge-note mt-4">
              <p><i>Lưu ý: Phụ phí sẽ được tính theo từng thành phần tùy chỉnh. Mỗi lần bạn thay đổi màu sắc hoặc áp dụng hình ảnh cho một thành phần, phụ phí tương ứng sẽ được cộng vào giá sản phẩm.</i></p>
            </div>
          </div>
        </div>
      </div>
      
      <!-- Modal chụp ảnh từ nhiều góc -->
      <div v-if="showCaptureModal" class="capture-modal">
        <div class="capture-modal-content">
          <div class="capture-modal-header">
            <h3>Chụp ảnh mô hình 3D</h3>
            <button class="close-button" @click="showCaptureModal = false">×</button>
          </div>
          <div class="capture-modal-body">
            <div class="preview-angles">
              <div 
                v-for="(angle, index) in captureAngles" 
                :key="index"
                :class="['preview-angle', { 'selected': selectedAngleIndex === index }]"
                @click="selectAngle(index)"
              >
                <div class="angle-preview">
                  <img v-if="angle.preview" :src="angle.preview" alt="Góc nhìn" />
                  <div v-else class="placeholder">
                    <span>{{ angle.name }}</span>
                  </div>
                </div>
                <div class="angle-name">{{ angle.name }}</div>
              </div>
            </div>
            
            <div class="capture-actions">
              <div class="download-controls">
                <button class="action-button" @click="downloadSelectedAngle">Tải ảnh đã chọn</button>
              </div>
            </div>
          </div>
        </div>
      </div>
      
      <!-- Modal hoàn thành thiết kế -->
      <div v-if="showCompleteModal" class="capture-modal">
        <div class="capture-modal-content">
          <div class="capture-modal-header">
            <h3>Hoàn thành thiết kế</h3>
            <button class="close-button" @click="showCompleteModal = false">×</button>
          </div>
          <div class="capture-modal-body">
            <div class="preview-angles">
              <div 
                v-for="(angle, index) in captureAngles" 
                :key="index"
                :class="['preview-angle', { 'selected': selectedAngleIndex === index }]"
                @click="selectAngle(index)"
              >
                <div class="angle-preview">
                  <img v-if="angle.preview" :src="angle.preview" alt="Góc nhìn" />
                  <div v-else class="placeholder">
                    <span>{{ angle.name }}</span>
                  </div>
                </div>
                <div class="angle-name">{{ angle.name }}</div>
              </div>
            </div>
            
            <div class="product-summary">
              <h4>Chi tiết sản phẩm</h4>
              <div class="summary-info">
                <p><strong>Tên sản phẩm:</strong> {{ customProductName }}</p>
                <p><strong>Giá gốc:</strong> {{ formatPrice(basePrice) }}</p>
                <p v-if="surcharge > 0"><strong>Phụ phí tùy chỉnh:</strong> {{ formatPrice(surcharge) }}</p>
                <p><strong>Tổng tiền:</strong> {{ formatPrice(basePrice + surcharge) }}</p>
              </div>
            </div>
            
            <div class="complete-actions">
              <button class="action-button" @click="saveAsDraft">Lưu nháp</button>
              <button class="action-button primary-button" @click="addToCart">Thêm vào giỏ hàng</button>
            </div>
          </div>
        </div>
      </div>
      
      <!-- Modal sửa tên sản phẩm -->
      <div v-if="showEditNameModal" class="edit-name-modal">
        <div class="edit-name-modal-content">
          <div class="edit-name-modal-header">
            <h3>Sửa tên sản phẩm</h3>
            <button class="close-button" @click="showEditNameModal = false">×</button>
          </div>
          <div class="edit-name-modal-body">
            <div class="form-group">
              <label for="productName">Tên sản phẩm:</label>
              <input type="text" id="productName" v-model="customProductName" class="form-control" placeholder="Nhập tên sản phẩm mới" />
            </div>
            <div class="edit-name-modal-actions">
              <button class="btn btn-secondary" @click="showEditNameModal = false">Hủy</button>
              <button class="btn btn-primary" @click="updateProductName">Lưu</button>
            </div>
          </div>
        </div>
      </div>
      
      <!-- Canvas for the 3D model -->
      <div class="model-container-wrapper" :class="{ 'expanded': isCanvasExpanded }">
        <div class="canvas-resizer">
          <button class="resize-handle" @click="toggleCanvasSize" :title="isCanvasExpanded ? 'Thu nhỏ' : 'Mở rộng'">
            <span class="resize-icon">{{ isCanvasExpanded ? '▲' : '▼' }}</span>
          </button>
        </div>
        <div ref="container" class="model-container"></div>
      </div>
      
      <div class="customizer-container">
        <!-- Components UI bên trái -->
        <div class="components-card">
          <h3 class="card-title">COMPONENTS</h3>
          <div class="components-dropdown-container">
            <select 
              class="components-dropdown"
              v-model="selectedComponentIndex"
              @change="handleComponentChange"
            >
              <option 
                v-for="(component, index) in components" 
                :key="component.name" 
                :value="index"
              >
                {{ component.name }}
              </option>
            </select>
          </div>
          
          <div class="selected-component-info">
            <div class="component-name">{{ components[selectedComponentIndex].name }}</div>
            <div class="component-index">{{ selectedComponentIndex + 1 }}/{{ components.length }}</div>
          </div>
        </div>
        
        <!-- Card hiển thị ảnh đã chọn -->
        <div v-if="selectedImage && activeTab === 'image'" class="image-preview-card">
          <div class="image-preview-container">
            <img :src="previewImageUrl" alt="Preview" class="image-preview-large" />
            <div class="image-navigation-buttons">
              <button 
                v-if="uploadedImageHistory.length > 1 && currentImageIndex > 0" 
                @click="showPreviousImage" 
                class="back-image-button"
                title="Xem ảnh trước đó"
              >
                ←
              </button>
              <button 
                v-if="uploadedImageHistory.length > 1 && currentImageIndex < uploadedImageHistory.length - 1" 
                @click="showNextImage" 
                class="next-image-button"
                title="Xem ảnh tiếp theo"
              >
                →
              </button>
            </div>
            <div v-if="uploadedImageHistory.length > 1" class="image-counter">
              {{ currentImageIndex + 1 }}/{{ uploadedImageHistory.length }}
            </div>
          </div>
        </div>
        
        <!-- Tùy chỉnh màu và texture ở giữa -->
        <div class="customizer-card">
          <h3 class="card-title">TÙY CHỈNH</h3>
          
          <div class="customizer-tabs">
            <button 
              :class="{'tab-button': true, 'active': activeTab === 'color'}" 
              @click="activeTab = 'color'"
            >
              Màu nền
            </button>
            <button 
              :class="{'tab-button': true, 'active': activeTab === 'image'}" 
              @click="activeTab = 'image'"
            >
              Hình ảnh
            </button>
            <button 
              :class="{'tab-button': true, 'active': activeTab === 'ai'}" 
              @click="activeTab = 'ai'"
            >
              AI Generate
            </button>
          </div>
          
          <!-- Color swatches -->
          <div v-if="activeTab === 'color'" class="tab-content">
            <div class="color-picker-container">
              <div class="color-picker-wrapper">
                <input 
                  type="color" 
                  v-model="customColorValue" 
                  @input="handleCustomColorChange"
                  class="color-picker-input" 
                />
                <button 
                  class="apply-custom-color-btn"
                  @click="applyCustomColor"
                >
                  Áp dụng
                </button>
              </div>
            </div>
          </div>
          
          <!-- Image upload -->
          <div v-if="activeTab === 'image'" class="tab-content">
            <div class="image-customizer">
              <div class="image-input-wrapper">
                <input 
                  type="file" 
                  ref="imageInput"
                  accept="image/*"
                  @change="onImageSelected"
                  class="image-input"
                  id="image-upload"
                />
                <label for="image-upload" class="upload-button">
                  Chọn ảnh
                </label>
                <div class="image-buttons">
                  <button class="text-button apply-text" @click="applyImageToMesh" :disabled="!selectedImage" title="Áp dụng ảnh vào phần đã chọn">
                    Áp dụng
                  </button>
                  <button class="text-button remove-text" @click="removeImagePreview" title="Xóa ảnh đã chọn">
                    Xóa
                  </button>
                </div>
              </div>
            </div>
          </div>
  
          <!-- AI Generate -->
          <div v-if="activeTab === 'ai'" class="tab-content">
            <div class="ai-form">
              <div class="form-group">
                <label>Prompt</label>
                <input
                  v-model="aiPrompt"
                  type="text"
                  class="input-text"
                  placeholder="Enter your prompt here..."
                />
              </div>
  
              <button @click="generateAIImage" class="submit-button" :disabled="isGenerating">
                {{ isGenerating ? 'Generating...' : 'Generate Image' }}
              </button>
  
              <!-- AI Generated Image Result -->
              <div v-if="generatedAIImage" class="result-section">
                <div class="image-container">
                  <img :src="generatedAIImage" alt="Generated AI Image" class="generated-image"/>
                  <div class="image-buttons">
                    <button class="text-button apply-text" @click="moveToImageSection" title="Chuyển sang phần Hình ảnh">
                      Chuyển sang Hình ảnh
                    </button>
                    <button class="text-button remove-text" @click="removeAIImage" title="Xóa ảnh đã tạo">
                      Xóa
                    </button>
                  </div>
                </div>
              </div>
  
              <!-- Error Message -->
              <div v-if="aiError" class="error-message">
                {{ aiError }}
              </div>
            </div>
          </div>
        </div>
        
        <!-- Công cụ texture bên phải -->
        <div v-if="activeTab === 'image'" class="texture-controls-card">
          <div class="texture-header">
            <h3 class="card-title">TEXTURE</h3>
          </div>
          <div class="texture-controls">
            <div class="texture-controls-grid">
              <div class="texture-control">
                <div class="control-label">scale</div>
                <div class="control-slider">
                  <input 
                    type="range" 
                    min="0.1" 
                    max="3.0" 
                    step="0.1" 
                    v-model.number="textureParams.scale"
                    class="slider" 
                  />
                </div>
                <div class="control-value">{{ Number(textureParams.scale).toFixed(1) }}</div>
              </div>
              
              <div class="texture-control">
                <div class="control-label">repeatX</div>
                <div class="control-slider">
                  <input 
                    type="range" 
                    min="0.1" 
                    max="5.0" 
                    step="0.1" 
                    v-model.number="textureParams.repeatX"
                    class="slider" 
                  />
                </div>
                <div class="control-value">{{ Number(textureParams.repeatX).toFixed(1) }}</div>
              </div>
              
              <div class="texture-control">
                <div class="control-label">repeatY</div>
                <div class="control-slider">
                  <input 
                    type="range" 
                    min="0.1" 
                    max="5.0" 
                    step="0.1" 
                    v-model.number="textureParams.repeatY"
                    class="slider" 
                  />
                </div>
                <div class="control-value">{{ Number(textureParams.repeatY).toFixed(1) }}</div>
              </div>
              
              <div class="texture-control">
                <div class="control-label">offsetX</div>
                <div class="control-slider">
                  <input 
                    type="range" 
                    min="-2.0" 
                    max="2.0" 
                    step="0.1" 
                    v-model.number="textureParams.offsetX"
                    class="slider" 
                  />
                </div>
                <div class="control-value">{{ Number(textureParams.offsetX).toFixed(1) }}</div>
              </div>
              
              <div class="texture-control">
                <div class="control-label">offsetY</div>
                <div class="control-slider">
                  <input 
                    type="range" 
                    min="-2.0" 
                    max="2.0" 
                    step="0.1" 
                    v-model.number="textureParams.offsetY"
                    class="slider" 
                  />
                </div>
                <div class="control-value">{{ Number(textureParams.offsetY).toFixed(1) }}</div>
              </div>
              
              <div class="texture-control">
                <div class="control-label">rotation</div>
                <div class="control-slider">
                  <input 
                    type="range" 
                    min="0" 
                    max="6.28" 
                    step="0.01" 
                    v-model.number="textureParams.rotation"
                    class="slider" 
                  />
                </div>
                <div class="control-value">{{ Number(textureParams.rotation).toFixed(2) }}</div>
              </div>
              
              <div class="texture-control">
                <div class="control-label">brightness</div>
                <div class="control-slider">
                  <input 
                    type="range" 
                    min="0.5" 
                    max="3.0" 
                    step="0.1" 
                    v-model.number="textureParams.brightness"
                    class="slider" 
                  />
                </div>
                <div class="control-value">{{ Number(textureParams.brightness).toFixed(2) }}</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  
    <!-- Modal xác nhận chuyển tab -->
    <div v-if="showConfirmModal" class="capture-modal">
      <div class="capture-modal-content">
        <div class="capture-modal-header">
          <h3>Xác nhận chuyển tab</h3>
          <button class="close-button" @click="showConfirmModal = false">×</button>
        </div>
        <div class="capture-modal-body">
          <p>AI đã tạo xong hình ảnh. Bạn có muốn chuyển sang tab hình ảnh để áp dụng không?</p>
          <div class="complete-actions">
            <button class="action-button" @click="showConfirmModal = false">Hủy</button>
            <button class="action-button primary-button" @click="handleConfirmSwitchTab">Chuyển tab</button>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted, onBeforeUnmount, reactive, watch, computed } from 'vue'
  import * as THREE from 'three'
  import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls'
  import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader'
  import { DRACOLoader } from 'three/examples/jsm/loaders/DRACOLoader'
  import { useRoute, useRouter } from 'vue-router'
  import aiService from '@/server/ai-service'
  import { getTemplateById } from '~/server/custom-service'
  
  // Container reference và state
  const container = ref(null)
  const surcharge = ref(0)
  const isCanvasExpanded = ref(false)
  const router = useRouter()
  
  // Modal states
  const showCaptureModal = ref(false)
  const showCompleteModal = ref(false)
  const showSurchargeInfo = ref(false)
  const showEditNameModal = ref(false)
  const selectedAngleIndex = ref(0)
  const customProductName = ref('')
  
  // Mở modal sửa tên sản phẩm
  const openEditNameModal = () => {
    if (!customProductName.value) {
      customProductName.value = 'Custom Running Shoes'
    }
    showEditNameModal.value = true
  }
  
  // Cập nhật tên sản phẩm
  const updateProductName = () => {
    if (customProductName.value.trim() === '') {
      customProductName.value = 'Custom Running Shoes'
    }
    showEditNameModal.value = false
  }
  
  // Theo dõi modal để khóa/mở scroll
  watch(showEditNameModal, (newValue) => {
    if (newValue) {
      // Khóa scroll khi modal hiển thị
      document.body.style.overflow = 'hidden'
    } else {
      // Khôi phục scroll khi modal đóng
      document.body.style.overflow = ''
    }
  })
  
  // Danh sách nhà sản xuất với các mức phụ phí khác nhau
  const selectedManufacturer = ref('adidas')
  const manufacturers = reactive([
    {
      id: 'adidas',
      name: 'Adidas',
      basePrice: 2500000,
      surcharges: {
        colorChange: 30000,     // Phụ phí khi thay đổi màu sắc một thành phần
        imageApplication: 50000, // Phụ phí khi áp dụng hình ảnh lên một thành phần
        componentRates: {
          // Hệ số nhân phụ phí cho từng thành phần
          Base: 1.0,
          Heel: 1.2,
          Lace: 0.8,
          OutSode: 1.5,
          MidSole: 1.3,
          Tip: 0.9,
          Accent: 1.1,
          Logo: 2.0,  // Logo có phụ phí cao hơn
          Details: 0.7
        }
      },
      modelPath: '/Adidasrunningshoes.glb'
    },
    {
      id: 'nike',
      name: 'Nike',
      basePrice: 2800000,
      surcharges: {
        colorChange: 35000,
        imageApplication: 60000,
        componentRates: {
          Base: 1.2,
          Heel: 1.5,
          Lace: 0.9,
          OutSode: 1.8,
          MidSole: 1.5,
          Tip: 1.0,
          Accent: 1.3,
          Logo: 2.5,  // Nike logo đắt hơn
          Details: 0.8
        }
      },
      modelPath: '/Adidasrunningshoes.glb' // Thay bằng đường dẫn mô hình Nike thực tế
    },
    {
      id: 'vans',
      name: 'Vans',
      basePrice: 1800000,
      surcharges: {
        colorChange: 25000,
        imageApplication: 40000,
        componentRates: {
          Base: 0.9,
          Heel: 1.0,
          Lace: 0.7,
          OutSode: 1.2,
          MidSole: 1.0,
          Tip: 0.8,
          Accent: 0.9,
          Logo: 1.8,
          Details: 0.6
        }
      },
      modelPath: '/Adidasrunningshoes.glb' // Thay bằng đường dẫn mô hình Vans thực tế
    }
  ])
  
  // Lấy thông tin nhà sản xuất hiện tại
  const currentManufacturer = computed(() => {
    return manufacturers.find(m => m.id === selectedManufacturer.value) || manufacturers[0]
  })
  
  const captureAngles = reactive([
    { name: 'Mặt sau', preview: null, position: { x: -6, y: 2, z: -6 } },
    { name: 'Bên trái', preview: null, position: { x: -4, y: 0, z: 6 } },
    { name: 'Bên phải', preview: null, position: { x: 4, y: 0, z: -6 } },
    { name: 'Góc nhìn trên', preview: null, position: { x: 4, y: 8, z: 4 } },
    { name: 'Góc nhìn dưới', preview: null, position: { x: -4, y: -12, z: 4 } }
  ])
  
  // Camera position storage
  let originalCameraPosition = null
  
  // Three.js objects
  let scene, camera, renderer, controls, model
  const materials = reactive({})
  const customTextures = reactive({})
  
  // State for customization
  const customText = ref('')
  const activeTab = ref('color')
  const selectedTextColor = ref('#ffffff')
  const textureParams = reactive({
    scale: 1.0,
    repeatX: 1.1,
    repeatY: 1.6,
    offsetX: -0.01,
    offsetY: -0.08,
    rotation: 0.24,
    brightness: 2.0
  })
  
  // Image upload state
  const imageInput = ref(null)
  const selectedImage = ref(null)
  const selectedImageName = ref('')
  const previewImageUrl = ref('')
  // Lịch sử ảnh đã upload
  const uploadedImageHistory = reactive([])
  const currentImageIndex = ref(-1)
  
  // Part colors and textures
  const partColors = reactive({
    Accent_inside: '#ffffff', Accent_outside: '#ffffff',
    Base: '#ffffff', Cover: '#ffffff',
    Cylinder: '#ffffff', Cylinder001: '#ffffff',
    Heel: '#ffffff', Lace: '#ffffff',
    Line_inside: '#ffffff', Line_outside: '#ffffff',
    Logo_inside: '#ffffff', Logo_outside: '#ffffff',
    MidSode: '#ffffff', MidSode001: '#ffffff',
    OutSode: '#ffffff', Tip: '#ffffff',
    Plane012: '#ffffff', Plane012_1: '#ffffff',
    Plane005: '#ffffff', Tongue: '#ffffff'
  })
  
  const partTextures = reactive({
    Accent_inside: null, Accent_outside: null,
    Base: null, Cover: null,
    Cylinder: null, Cylinder001: null,
    Heel: null, Lace: null,
    Line_inside: null, Line_outside: null,
    Logo_inside: null, Logo_outside: null,
    MidSode: null, MidSode001: null,
    OutSode: null, Tip: null,
    Plane012: null, Plane012_1: null,
    Plane005: null, Tongue: null
  })
  
  // Components list price color, price texture, hãng adidas
  const components = reactive([
    { name: 'Base', value: 'Base' },
    { name: 'Heel', value: 'Heel' },
    { name: 'Lace', value: 'Lace' },
    { name: 'Outsole', value: 'OutSode' },
    { name: 'Midsole', value: 'MidSole'},
    { name: 'Tip', value: 'Tip' },
    { name: 'Accent', value: 'Accent' },
    { name: 'Logo', value: 'Logo' },
    { name: 'Details', value: 'Details' }
  ])
  
  // Part groups 
  const partGroups = reactive({
    Accent: ['Accent_inside', 'Accent_outside', 'Line_inside', 'Line_outside'],
    Logo: ['Logo_inside', 'Logo_outside'],
    MidSole: ['MidSode', 'MidSode001', 'Plane012', 'Plane012_1'], 
    Details: ['Cylinder', 'Cylinder001', 'Plane005', 'Cover']
  })
  
  // Selected component and color
  const selectedComponentIndex = ref(0)
  const selectedColor = ref('#000000')
  const customColorValue = ref('#ffffff')
  const customColorApplied = ref(false)
  
  // Modal handlers
  const openCaptureModal = () => {
    originalCameraPosition = {
      position: camera ? camera.position.clone() : null,
      rotation: controls ? controls.target.clone() : null
    }
    showCaptureModal.value = true
    captureAllAngles()
  }
  
  const selectAngle = (index) => {
    selectedAngleIndex.value = index
    if (camera && controls) {
      const anglePosition = captureAngles[index].position
      const startPosition = camera.position.clone()
      const targetPosition = new THREE.Vector3(anglePosition.x, anglePosition.y, anglePosition.z)
      controls.target.set(0, 0, 0)
      const duration = 500
      const startTime = Date.now()
  
      function animateCamera() {
        const elapsed = Date.now() - startTime
        const progress = Math.min(elapsed / duration, 1)
        const easeProgress = progress < 0.5 ? 2 * progress * progress : 1 - Math.pow(-2 * progress + 2, 2) / 2
        camera.position.lerpVectors(startPosition, targetPosition, easeProgress)
        controls.update()
        if (progress < 1) {
          requestAnimationFrame(animateCamera)
        } else {
          captureCurrentAngle()
        }
      }
      animateCamera()
    }
  }
  
  const captureCurrentAngle = () => {
    if (renderer) {
      renderer.render(scene, camera)
      captureAngles[selectedAngleIndex.value].preview = renderer.domElement.toDataURL('image/png')
    }
  }
  
  const captureAllAngles = async () => {
    const initialPosition = camera.position.clone()
    const initialTarget = controls.target.clone()
    controls.target.set(0, 0, 0)
  
    for (let i = 0; i < captureAngles.length; i++) {
      selectedAngleIndex.value = i
      const anglePosition = captureAngles[i].position
      camera.position.set(anglePosition.x, anglePosition.y, anglePosition.z)
      controls.update()
      await new Promise(resolve => setTimeout(resolve, 300))
      renderer.render(scene, camera)
      captureAngles[i].preview = renderer.domElement.toDataURL('image/png')
    }
  
    camera.position.copy(initialPosition)
    controls.target.copy(initialTarget)
    controls.update()
    selectedAngleIndex.value = 0
  }
  
  const downloadSelectedAngle = () => {
    const selectedAngle = captureAngles[selectedAngleIndex.value]
    if (!selectedAngle.preview) {
      captureCurrentAngle()
    }
    const link = document.createElement('a')
    link.href = selectedAngle.preview
    link.download = `adidas-custom-shoe-${selectedAngle.name}.png`
    document.body.appendChild(link)
    link.click()
    document.body.removeChild(link)
  }
  
  const handleDone = () => {
    originalCameraPosition = {
      position: camera ? camera.position.clone() : null,
      rotation: controls ? controls.target.clone() : null
    }
    calculateSurcharge()
    
    captureAllAngles().then(() => {
      showCompleteModal.value = true
    })
  }
  
  const onImageSelected = (event) => {
    const file = event.target.files[0]
    if (file) {
      selectedImage.value = file
      selectedImageName.value = file.name.length > 20 ? file.name.substring(0, 17) + '...' : file.name
      
      const reader = new FileReader()
      reader.onload = (e) => {
        previewImageUrl.value = e.target.result
        
        // Lưu ảnh vào lịch sử
        uploadedImageHistory.push({
          file: file,
          name: selectedImageName.value,
          imageUrl: e.target.result
        })
        currentImageIndex.value = uploadedImageHistory.length - 1
      }
      reader.readAsDataURL(file)
    }
  }
  
  // Hàm xóa ảnh preview
  const removeImagePreview = () => {
    if (selectedImage.value) {
      // Xóa ảnh hiện tại ra khỏi lịch sử nếu cần
      if (uploadedImageHistory.length > 0 && currentImageIndex.value >= 0) {
        uploadedImageHistory.splice(currentImageIndex.value, 1);
        
        // Hiển thị ảnh mới nhất trong lịch sử nếu còn
        if (uploadedImageHistory.length > 0) {
          currentImageIndex.value = uploadedImageHistory.length - 1;
          const latestImage = uploadedImageHistory[currentImageIndex.value];
          selectedImage.value = latestImage.file;
          selectedImageName.value = latestImage.name;
          previewImageUrl.value = latestImage.imageUrl;
          return; // Không xóa hết vì vẫn còn ảnh trong lịch sử
        } else {
          currentImageIndex.value = -1;
        }
      }
    }
    
    // Nếu không còn ảnh trong lịch sử hoặc đã xóa hết
    selectedImage.value = null;
    selectedImageName.value = '';
    previewImageUrl.value = '';
    if (imageInput.value) {
      imageInput.value.value = '';
    }
  }
  
  const applyImageToMesh = () => {
    if (!selectedImage.value) {
      alert('Vui lòng chọn ảnh trước khi áp dụng')
      return
    }
  
    const selectedPart = components[selectedComponentIndex.value].value
    const partsToUpdate = selectedPart in partGroups ? partGroups[selectedPart] : [selectedPart]
  
    const imageUrl = previewImageUrl.value
    const textureLoader = new THREE.TextureLoader()
    textureLoader.load(
      imageUrl,
      (texture) => {
        texture.anisotropy = renderer.capabilities.getMaxAnisotropy()
        texture.wrapS = texture.wrapT = THREE.RepeatWrapping
        texture.flipY = false
        texture.encoding = THREE.sRGBEncoding
        texture.repeat.set(textureParams.repeatX * textureParams.scale, textureParams.repeatY * textureParams.scale)
        texture.offset.set(textureParams.offsetX, textureParams.offsetY)
        texture.rotation = textureParams.rotation
        texture.needsUpdate = true
  
        if (partsToUpdate.includes('Lace')) {
          const laceMeshes = findAllLaceMeshes()
          
          if (laceMeshes.length > 0) {
            laceMeshes.forEach(mesh => {
              const newMaterial = new THREE.MeshStandardMaterial({
                map: texture,
                color: new THREE.Color(textureParams.brightness, textureParams.brightness, textureParams.brightness),
                transparent: true,
                side: THREE.DoubleSide,
                metalness: 0.3,
                roughness: 0.4
              })
              
              if (!customTextures['Lace']) {
                customTextures['Lace'] = {
                  originalMap: mesh.material.map,
                  originalColor: mesh.material.color ? mesh.material.color.clone() : new THREE.Color('#ffffff'),
                  texture,
                  imageData: imageUrl
                }
              }
              
              mesh.material = newMaterial
              mesh.material.needsUpdate = true
              materials['Lace'] = newMaterial
              partTextures['Lace'] = texture
            })
          }
        }
  
        partsToUpdate.forEach((part) => {
          if (part !== 'Lace' && materials[part]) {
            if (!customTextures[part]) {
              customTextures[part] = {
                originalMap: materials[part].map,
                originalColor: materials[part].color.clone(),
                texture,
                imageData: imageUrl
              }
            } else {
              customTextures[part].texture = texture
              customTextures[part].imageData = imageUrl
            }
            partTextures[part] = texture
            materials[part].map = texture
            materials[part].color.set(new THREE.Color(textureParams.brightness, textureParams.brightness, textureParams.brightness))
            materials[part].transparent = true
            materials[part].needsUpdate = true
            materials[part].metalness = 0.3
            materials[part].roughness = 0.4
          }
        })
  
        renderer.render(scene, camera)
        calculateSurcharge()
      },
      undefined,
      (error) => {
        alert('Đã xảy ra lỗi khi tải ảnh, vui lòng thử lại')
      }
    )
  }
  
  const applyTextToMesh = () => {
    if (!customText.value.trim()) {
      alert('Vui lòng nhập văn bản trước khi áp dụng')
      return
    }
  
    const selectedPart = components[selectedComponentIndex.value].value
    const partsToUpdate = selectedPart in partGroups ? partGroups[selectedPart] : [selectedPart]
  
    const canvas = document.createElement('canvas')
    canvas.width = 1024
    canvas.height = 1024
    const context = canvas.getContext('2d')
  
    context.fillStyle = '#ffffff'
    context.fillRect(0, 0, canvas.width, canvas.height)
  
    context.save()
    context.translate(canvas.width / 2, canvas.height / 2)
    context.rotate(Math.PI)
    context.scale(-1, 1)
    context.translate(-canvas.width / 2, -canvas.height / 2)
  
    const textLength = customText.value.length
    const fontSize = Math.min(150, 600 / Math.max(1, textLength / 3))
    context.font = `bold ${fontSize}px Arial, sans-serif`
    context.strokeStyle = 'black'
    context.lineWidth = fontSize / 8
    context.textAlign = 'center'
    context.textBaseline = 'middle'
    context.strokeText(customText.value, canvas.width / 2, canvas.height / 2)
    context.fillStyle = '#ffffff'
    context.shadowColor = 'rgba(0, 0, 0, 0.7)'
    context.shadowBlur = 3
    context.shadowOffsetX = 1
    context.shadowOffsetY = 1
    context.fillText(customText.value, canvas.width / 2, canvas.height / 2)
    context.shadowColor = 'transparent'
    context.fillText(customText.value, canvas.width / 2, canvas.height / 2)
  
    context.restore()
  
    const texture = new THREE.CanvasTexture(canvas)
    texture.anisotropy = renderer.capabilities.getMaxAnisotropy()
    texture.wrapS = texture.wrapT = THREE.RepeatWrapping
    texture.repeat.set(1, 1)
    texture.needsUpdate = true
  
    if (partsToUpdate.includes('Lace')) {
      const laceMeshes = findAllLaceMeshes()
      
      if (laceMeshes.length > 0) {
        laceMeshes.forEach(mesh => {
          const newMaterial = new THREE.MeshStandardMaterial({
            map: texture,
            transparent: true,
            side: THREE.DoubleSide,
            metalness: 0.3,
            roughness: 0.4
          })
          
          if (!customTextures['Lace']) {
            customTextures['Lace'] = {
              originalMap: mesh.material.map,
              originalColor: mesh.material.color ? mesh.material.color.clone() : new THREE.Color('#ffffff'),
              texture
            }
          } else {
            customTextures['Lace'].texture = texture
          }
          
          mesh.material = newMaterial
          mesh.material.needsUpdate = true
          materials['Lace'] = newMaterial
          partTextures['Lace'] = texture
        })
      }
    }
  
    partsToUpdate.forEach((part) => {
      if (part !== 'Lace' && materials[part]) {
        if (!customTextures[part]) {
          customTextures[part] = {
            originalMap: materials[part].map,
            originalColor: materials[part].color.clone(),
            texture
          }
        } else {
          customTextures[part].texture = texture
        }
        partTextures[part] = texture
        materials[part].map = texture
        materials[part].needsUpdate = true
        materials[part].metalness = 0.3
        materials[part].roughness = 0.4
      }
    })
  
    renderer.render(scene, camera)
    calculateSurcharge()
  }
  
  const removeTextFromMesh = () => {
    const selectedPart = components[selectedComponentIndex.value].value
    const partsToUpdate = selectedPart in partGroups ? partGroups[selectedPart] : [selectedPart]
  
    if (partsToUpdate.includes('Lace')) {
      const laceMeshes = findAllLaceMeshes()
      
      if (laceMeshes.length > 0) {
        laceMeshes.forEach(mesh => {
          if (customTextures['Lace']) {
            const newMaterial = new THREE.MeshStandardMaterial({
              map: customTextures['Lace'].originalMap,
              side: THREE.DoubleSide,
              metalness: 0.3,
              roughness: 0.4
            })
            
            if (customTextures['Lace'].originalColor) {
              newMaterial.color.copy(customTextures['Lace'].originalColor)
            }
            
            mesh.material = newMaterial
            mesh.material.needsUpdate = true
          }
        })
        partTextures['Lace'] = null
        delete customTextures['Lace']
      }
    }
    
    partsToUpdate.forEach((part) => {
      if (part !== 'Lace' && materials[part] && customTextures[part]) {
        materials[part].map = customTextures[part].originalMap
        materials[part].color.copy(customTextures[part].originalColor)
        materials[part].transparent = materials[part].map ? true : false
        materials[part].needsUpdate = true
        materials[part].metalness = 0.3
        materials[part].roughness = 0.4
        partTextures[part] = null
        delete customTextures[part]
      }
    })
  
    if (imageInput.value) {
      imageInput.value.value = ''
    }
    if (activeTab.value === 'image') {
      if (previewImageUrl.value) {
        previewImageUrl.value = ''
      }
      selectedImage.value = null
      selectedImageName.value = ''
    }
  
    renderer.render(scene, camera)
    calculateSurcharge()
  }
  
  const addToCart = () => {
    showCompleteModal.value = false
    const urlParams = new URLSearchParams(window.location.search)
    const isEditing = urlParams.get('edit') === 'true'
    const editId = urlParams.get('id')
    
    calculateSurcharge()
    
    const productData = {
      id: isEditing && editId ? parseInt(editId) : Date.now(),
      name: customProductName.value || 'Custom Running Shoes',
      manufacturerId: selectedManufacturer.value,
      price: basePrice.value, // Giá cố định
      surcharge: surcharge.value,
      image: captureAngles[1].preview,
      designData: {
        colors: {},
        textures: {},
        imagesData: {},
        customText: customText.value,
        textureParams: { ...textureParams },
        timestamp: new Date().toISOString(),
        manufacturerId: selectedManufacturer.value
      },
      previewImages: captureAngles.map(angle => angle.preview)
    }
    
    for (const comp of components) {
      const partName = comp.value
      const partsToSave = partGroups[partName] || [partName]
      partsToSave.forEach((subPart) => {
        if (materials[subPart]) {
          const hexColor = '#' + materials[subPart].color.getHexString()
          productData.designData.colors[subPart] = hexColor
          
          if (customTextures[subPart]) {
            const textureType = customTextures[subPart].texture instanceof THREE.CanvasTexture ? 'text' : 'image'
            productData.designData.textures[subPart] = {
              type: textureType,
              textContent: customText.value
            }
            if (textureType === 'image' && customTextures[subPart].imageData) {
              productData.designData.imagesData[subPart] = customTextures[subPart].imageData
            }
          }
        }
      })
    }
    
    let cart = []
    const savedCart = localStorage.getItem('cart')
    if (savedCart) {
      cart = JSON.parse(savedCart)
    }
    
    const totalPrice = basePrice.value + surcharge.value // Giá cố định + phụ phí
    const formattedTotalPrice = formatPrice(totalPrice)
    const formattedSurcharge = surcharge.value > 0 ? `\nPhụ phí tùy chỉnh: ${formatPrice(surcharge.value)}` : ''
    
    if (isEditing && editId) {
      const itemIndex = cart.findIndex(item => item.id === parseInt(editId))
      if (itemIndex !== -1) {
        cart[itemIndex] = productData
      } else {
        cart.push(productData)
      }
      alert(`Đã cập nhật thiết kế của sản phẩm trong giỏ hàng!\nGiá gốc: ${formatPrice(basePrice.value)}${formattedSurcharge}\nTổng tiền: ${formattedTotalPrice}`)
    } else {
      cart.push(productData)
      alert(`Sản phẩm thiết kế đã được thêm vào giỏ hàng thành công!\nGiá gốc: ${formatPrice(basePrice.value)}${formattedSurcharge}\nTổng tiền: ${formattedTotalPrice}`)
    }
    
    localStorage.setItem('cart', JSON.stringify(cart))
    window.location.href = '/cartcustomPage'
  }
  
  const saveAsDraft = () => {
    showCompleteModal.value = false
    calculateSurcharge()
    
    const productData = {
      id: Date.now(),
      name: customProductName.value || 'Custom Running Shoes (Nháp)',
      manufacturerId: selectedManufacturer.value,
      price: basePrice.value, // Giá cố định
      surcharge: surcharge.value,
      image: captureAngles[1].preview,
      designData: {
        colors: {},
        textures: {},
        imagesData: {},
        customText: customText.value,
        textureParams: { ...textureParams },
        timestamp: new Date().toISOString(),
        manufacturerId: selectedManufacturer.value
      },
      previewImages: captureAngles.map(angle => angle.preview)
    }
    
    for (const comp of components) {
      const partName = comp.value
      const partsToSave = partGroups[partName] || [partName]
      partsToSave.forEach((subPart) => {
        if (materials[subPart]) {
          const hexColor = '#' + materials[subPart].color.getHexString()
          productData.designData.colors[subPart] = hexColor
          if (customTextures[subPart]) {
            const textureType = customTextures[subPart].texture instanceof THREE.CanvasTexture ? 'text' : 'image'
            productData.designData.textures[subPart] = {
              type: textureType,
              textContent: customText.value
            }
            if (textureType === 'image' && customTextures[subPart].imageData) {
              productData.designData.imagesData[subPart] = customTextures[subPart].imageData
            }
          }
        }
      })
    }
    
    let drafts = []
    const savedDrafts = localStorage.getItem('designDrafts')
    if (savedDrafts) {
      drafts = JSON.parse(savedDrafts)
    }
    
    drafts.push(productData)
    localStorage.setItem('designDrafts', JSON.stringify(drafts))
    
    const totalPrice = basePrice.value + surcharge.value // Giá cố định + phụ phí
    const formattedTotalPrice = formatPrice(totalPrice)
    const formattedSurcharge = surcharge.value > 0 ? `\nPhụ phí tùy chỉnh: ${formatPrice(surcharge.value)}` : ''
    
    alert(`Thiết kế đã được lưu vào bản nháp!\nGiá gốc: ${formatPrice(basePrice.value)}${formattedSurcharge}\nTổng tiền: ${formattedTotalPrice}`)
    window.location.href = '/mycustomPage'
  }
  
  // Updated Three.js initialization
  const initThree = () => {
    scene = new THREE.Scene()
    scene.background = new THREE.Color(0xf0f0f0)
  
    camera = new THREE.PerspectiveCamera(75, container.value.clientWidth / container.value.clientHeight, 0.1, 1000)
    camera.position.set(0, 1, 8)
  
    renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true, preserveDrawingBuffer: true })
    renderer.setPixelRatio(window.devicePixelRatio)
    renderer.setSize(container.value.clientWidth, container.value.clientHeight)
    renderer.shadowMap.enabled = true
    renderer.shadowMap.type = THREE.PCFSoftShadowMap
    container.value.appendChild(renderer.domElement)
  
    // Lighting setup
    const ambientLight = new THREE.AmbientLight(0xffffff, 1.0)
    scene.add(ambientLight)
  
    const mainLight = new THREE.DirectionalLight(0xffffff, 1.5)
    mainLight.position.set(5, 10, 7)
    mainLight.castShadow = true
    mainLight.shadow.mapSize.width = 1024
    mainLight.shadow.mapSize.height = 1024
    scene.add(mainLight)
  
    const fillLight = new THREE.DirectionalLight(0xffffff, 0.8)
    fillLight.position.set(-5, 0, -5)
    scene.add(fillLight)
  
    controls = new OrbitControls(camera, renderer.domElement)
    controls.enableDamping = true
    controls.dampingFactor = 0.05
    controls.rotateSpeed = 0.7
    controls.minDistance = 3
    controls.maxDistance = 20
    controls.target.set(0, 0, 0)
  
    loadModel()
    animate()
    window.addEventListener('resize', onWindowResize)
  }
  
  const loadModel = () => {
    const dracoLoader = new DRACOLoader()
    dracoLoader.setDecoderPath('https://www.gstatic.com/draco/versioned/decoders/1.5.5/')
    const loader = new GLTFLoader()
    loader.setDRACOLoader(dracoLoader)
  
    // Ưu tiên dùng model3DUrl từ API, nếu không có thì fallback sang modelPath mặc định
    const modelPath = model3DUrl.value || currentManufacturer.value.modelPath
    loader.load(
      modelPath,
      (gltf) => onModelLoaded(gltf),
      (xhr) => null,
      (error) => alert('Lỗi khi tải mô hình 3D')
    )
  }
  
  const onModelLoaded = (gltf) => {
    if (model) {
      scene.remove(model)
    }
  
    model = gltf.scene
    model.scale.set(40, 40, 40)
    model.position.set(0, -2, 0)
    model.rotation.y = Math.PI / 4
  
    const foundMeshes = []
    const meshMaterialMap = {}
    const possibleLaceMeshes = []
    const originalMaterials = {}
  
    model.traverse((node) => {
      if (node.isMesh) {
        foundMeshes.push(node.name)
  
        node.castShadow = true
        node.receiveShadow = true
  
        if (!node.material) {
          node.material = new THREE.MeshStandardMaterial({ color: 0x808080 })
        }
  
        // Lưu trữ material gốc cho mesh cụ thể
        materials[node.name] = node.material.clone()
        meshMaterialMap[node.name] = node.material
        originalMaterials[node.name] = node.material.clone()
  
        if (
          node.name.toLowerCase().includes('lace') ||
          node.name.toLowerCase().includes('shoelace') ||
          node.name.toLowerCase().includes('string') ||
          node.name.toLowerCase().includes('cord')
        ) {
          possibleLaceMeshes.push({
            name: node.name,
            uuid: node.uuid,
            material: node.material ? node.material.type : 'không có material',
            materialColor: node.material && node.material.color ? node.material.color.getHexString() : 'không có màu',
          })
        }
  
        if (node.material.type === 'MeshStandardMaterial') {
          node.material.metalness = node.material.metalness !== undefined ? node.material.metalness : 0.3
          node.material.roughness = node.material.roughness !== undefined ? node.material.roughness : 0.4
          node.material.needsUpdate = true
        }
  
        if (!node.material.map && !node.material.color) {
          node.material.color = new THREE.Color(0xffffff)
          node.material.needsUpdate = true
        }
      }
    })
  
    // Áp dụng tùy chỉnh cho material của mỗi mesh
    Object.keys(partColors).forEach((partName) => {
      const matchingMesh = foundMeshes.find((meshName) => meshName.toLowerCase() === partName.toLowerCase())
      if (!matchingMesh) return
  
      const originalMaterial = originalMaterials[matchingMesh] || new THREE.MeshStandardMaterial({ color: 0xffffff })
  
      const texture = partTextures[partName]
      if (texture) {
        texture.repeat.set(textureParams.repeatX * textureParams.scale, textureParams.repeatY * textureParams.scale)
        texture.offset.set(textureParams.offsetX, textureParams.offsetY)
        texture.rotation = textureParams.rotation
        texture.wrapS = texture.wrapT = THREE.RepeatWrapping
        texture.needsUpdate = true
      }
  
      // Cập nhật material cho mesh cụ thể
      if (materials[matchingMesh]) {
        materials[matchingMesh].color = new THREE.Color(partColors[partName])
        materials[matchingMesh].map = texture || undefined
        materials[matchingMesh].transparent = !!texture
        materials[matchingMesh].metalness = 0.3
        materials[matchingMesh].roughness = 0.4
        materials[matchingMesh].needsUpdate = true
      } else {
        materials[matchingMesh] = originalMaterial.clone()
        materials[matchingMesh].color = new THREE.Color(partColors[partName])
        materials[matchingMesh].map = texture || undefined
        materials[matchingMesh].transparent = !!texture
        materials[matchingMesh].metalness = 0.3
        materials[matchingMesh].roughness = 0.4
        materials[matchingMesh].needsUpdate = true
      }
  
      // Áp dụng material cho mesh trùng khớp chính xác
      model.traverse((node) => {
        if (node.isMesh && node.name.toLowerCase() === partName.toLowerCase()) {
          node.material = materials[node.name]
          node.material.needsUpdate = true
        }
      })
    })
  
    checkPartGroups(foundMeshes)
    scene.add(model)
  }
  
  const checkPartGroups = (foundMeshes) => {
    const missingMeshes = []
    for (const groupName in partGroups) {
      partGroups[groupName].forEach(partName => {
        if (!foundMeshes.some(mesh => mesh.toLowerCase().includes(partName.toLowerCase()))) {
          missingMeshes.push(partName)
        }
      })
    }
  }
  
  const animate = () => {
    requestAnimationFrame(animate)
    controls.update()
    renderer.render(scene, camera)
  }
  
  const onWindowResize = () => {
    if (camera && renderer && container.value) {
      camera.aspect = container.value.clientWidth / container.value.clientHeight;
      camera.updateProjectionMatrix();
      renderer.setSize(container.value.clientWidth, container.value.clientHeight);
      renderer.render(scene, camera);
    }
  };
  
  const handleComponentChange = () => {
    console.log('Đã chọn component:', components[selectedComponentIndex.value].name)
  }
  
  const handleCustomColorChange = () => {
    selectedColor.value = customColorValue.value;
  };
  
  const applyCustomColor = () => {
    if (!model) {
      console.error('Model is not loaded. Cannot apply color.');
      return;
    }
  
    const selectedPart = components[selectedComponentIndex.value].value;
    const partsToUpdate = selectedPart in partGroups ? partGroups[selectedPart] : [selectedPart];
  
    console.log(`Applying color to component: ${selectedPart}, parts: ${partsToUpdate.join(', ')}`);
  
    // Handle Lace separately due to its unique mesh structure
    if (partsToUpdate.includes('Lace')) {
      console.log('Đang áp dụng màu cho dây giày:', customColorValue.value);
      const laceMeshes = findAllLaceMeshes()
  
      if (laceMeshes.length > 0) {
        laceMeshes.forEach(mesh => {
          console.log(`Đang áp dụng màu cho mesh ${mesh.name}`);
          if (!customTextures['Lace']) {
            customTextures['Lace'] = {
              originalMap: mesh.material.map,
              originalColor: mesh.material.color ? mesh.material.color.clone() : new THREE.Color('#ffffff')
            };
          }
  
          const newMaterial = new THREE.MeshStandardMaterial({
            color: new THREE.Color(customColorValue.value),
            side: THREE.DoubleSide,
            metalness: 0.3,
            roughness: 0.4
          });
  
          mesh.material = newMaterial;
          mesh.material.needsUpdate = true;
          materials['Lace'] = newMaterial;
  
          if ('Lace' in partColors) {
            partColors['Lace'] = customColorValue.value;
          } else {
            console.warn(`'Lace' not found in partColors. Adding it.`);
            partColors['Lace'] = customColorValue.value;
          }
  
          if (partTextures['Lace']) {
            partTextures['Lace'] = null;
          }
        });
      } else {
        console.warn('Không tìm thấy mesh dây giày nào');
      }
    }
  
    // Apply color to other parts
    partsToUpdate.forEach((part) => {
      if (part !== 'Lace') {
        let partUpdated = false;
  
        // Traverse the model and apply a unique material to each matching mesh
        model.traverse((node) => {
          if (node.isMesh) {
            // Exact match only: the mesh name must exactly match the part name
            const nodeNameLower = node.name.toLowerCase();
            const partLower = part.toLowerCase();
            const isExactMatch = nodeNameLower === partLower;
  
            if (isExactMatch) {
              console.log(`Đang áp dụng màu cho mesh: ${node.name}`);
  
              // Create a new material for this specific mesh
              const newMaterial = new THREE.MeshStandardMaterial({
                color: new THREE.Color(customColorValue.value),
                metalness: 0.3,
                roughness: 0.4
              });
  
              // Preserve existing properties if they exist
              if (node.material.map) {
                newMaterial.map = node.material.map;
                newMaterial.transparent = node.material.transparent;
              }
  
              // Assign the new material to the mesh
              node.material = newMaterial;
              node.material.needsUpdate = true;
  
              // Store the new material in materials with a unique key for this mesh
              materials[node.name] = newMaterial;
  
              partUpdated = true;
            }
          }
        });
  
        if (partUpdated) {
          // Update partColors if the part exists
          if (part in partColors) {
            partColors[part] = customColorValue.value;
          } else {
            console.warn(`Part '${part}' not found in partColors. Adding it.`);
            partColors[part] = customColorValue.value;
          }
  
          // Remove textures if they exist
          if (partTextures[part] && customTextures[part]) {
            // Update the material for each matching mesh
            model.traverse((node) => {
              if (node.isMesh && node.name.toLowerCase() === part.toLowerCase()) {
                node.material.map = undefined;
                node.material.transparent = false;
                node.material.needsUpdate = true;
              }
            });
            partTextures[part] = null;
            delete customTextures[part];
          }
        } else {
          console.warn(`Không tìm thấy mesh nào cho phần: ${part}`);
        }
      }
    });
  
    renderer.render(scene, camera);
    calculateSurcharge();
  };
  
  const toggleCanvasSize = () => {
    isCanvasExpanded.value = !isCanvasExpanded.value;
    setTimeout(() => {
      if (renderer && container.value) {
        onWindowResize();
      }
    }, 300);
  }
  
  const updateTextureParameters = () => {
    const selectedPart = components[selectedComponentIndex.value].value
    const partsToUpdate = selectedPart in partGroups ? partGroups[selectedPart] : [selectedPart]
  
    partsToUpdate.forEach((part) => {
      if (partTextures[part] && materials[part]) {
        const texture = partTextures[part]
        texture.repeat.set(textureParams.repeatX * textureParams.scale, textureParams.repeatY * textureParams.scale)
        texture.offset.set(textureParams.offsetX, textureParams.offsetY)
        texture.rotation = textureParams.rotation
        texture.needsUpdate = true
        
        if (customTextures[part] && customTextures[part].imageData) {
          materials[part].color.set(new THREE.Color(textureParams.brightness, textureParams.brightness, textureParams.brightness))
        }
        
        materials[part].needsUpdate = true
        materials[part].metalness = 0.3 // Match Code 1
        materials[part].roughness = 0.4 // Match Code 1
  
        model.traverse((node) => {
          if (node.isMesh && node.name.toLowerCase().includes(part.toLowerCase())) {
            node.material = materials[part]
            node.material.needsUpdate = true
          }
        })
      }
    })
    
    renderer.render(scene, camera)
  }
  
  watch(textureParams, updateTextureParameters, { deep: true })
  
  const route = useRoute();
  const templateId = route.params.id;
  const templateData = ref(null);
  
  // Thêm các biến reactive ở đầu script setup nếu chưa có
  const basePrice = ref(0);
  const model3DUrl = ref('');
  const description = ref('');
  const color = ref('');
  const gender = ref('');
  const isAvailable = ref(false);
  const createdAt = ref('');
  const updatedAt = ref('');
  
  onMounted(async () => {
    const response = await getTemplateById(templateId);
    console.log('Template response:', response);
    if (response) {
      templateData.value = response;
      customProductName.value = response.name || '';
      previewImageUrl.value = response.previewImageUrl || '';
      basePrice.value = response.basePrice || 0;
      model3DUrl.value = response.model3DUrl || '';
      description.value = response.description || '';
      color.value = response.color || '';
      gender.value = response.gender || '';
      isAvailable.value = response.isAvailable || false;
      createdAt.value = response.createdAt || '';
      updatedAt.value = response.updatedAt || '';
    }
    console.log('model3DUrl:', model3DUrl.value);
    initThree(); // GỌI SAU khi đã gán model3DUrl.value
  });
  
  onBeforeUnmount(() => {
    window.removeEventListener('resize', onWindowResize)
    if (model) {
      scene.remove(model)
      model.traverse((node) => {
        if (node.isMesh) {
          if (node.geometry) node.geometry.dispose()
          if (node.material) node.material.dispose()
        }
      })
    }
    if (renderer) {
      renderer.dispose()
      if (container.value) container.value.removeChild(renderer.domElement)
    }
    if (controls) controls.dispose()
    if (scene) scene.clear()
    if (previewImageUrl.value) URL.revokeObjectURL(previewImageUrl.value)
  })
  
  const formatPrice = (price) => {
    return new Intl.NumberFormat('vi-VN', {
      style: 'currency',
      currency: 'VND'
    }).format(price || 0);
  };
  
  const calculateSurcharge = () => {
    let totalSurcharge = 0
    const manufacturer = currentManufacturer.value
    
    // Khởi tạo mảng để theo dõi các thành phần đã tùy chỉnh
    const customizedComponents = {
      colors: [],
      textures: []
    }
    
    // Kiểm tra tất cả các thành phần đã tùy chỉnh
    for (const comp of components) {
      const partName = comp.value
      const partsToCheck = partGroups[partName] || [partName]
      
      partsToCheck.forEach((subPart) => {
        if (materials[subPart]) {
          // Kiểm tra nếu đã thay đổi màu sắc (không phải màu trắng mặc định)
          const hexColor = '#' + materials[subPart].color.getHexString()
          if (hexColor.toLowerCase() !== '#ffffff') {
            // Thêm vào danh sách đã tùy chỉnh để tránh tính trùng
            if (!customizedComponents.colors.includes(partName)) {
              customizedComponents.colors.push(partName)
              
              // Tính phụ phí theo thành phần và nhà sản xuất
              const componentFee = manufacturer.surcharges.colorChange * 
                                  (manufacturer.surcharges.componentRates[partName] || 1.0)
              totalSurcharge += componentFee
            }
          }
          
          // Kiểm tra nếu đã áp dụng texture
          if (customTextures[subPart]) {
            const textureType = customTextures[subPart].texture instanceof THREE.CanvasTexture ? 'text' : 'image'
            if (textureType === 'image' && !customizedComponents.textures.includes(partName)) {
              customizedComponents.textures.push(partName)
              
              // Phụ phí cho ứng dụng hình ảnh
              const textureFee = manufacturer.surcharges.imageApplication * 
                                (manufacturer.surcharges.componentRates[partName] || 1.0)
              totalSurcharge += textureFee
            }
          }
        }
      })
    }
    
    // Cập nhật giá trị phụ phí
    surcharge.value = Math.round(totalSurcharge)
    
    // Cập nhật hiển thị phụ phí
    updateSurchargeDisplay()
  }
  
  // Hàm hiển thị phụ phí
  const updateSurchargeDisplay = () => {
    const productSurchargeElement = document.querySelector('.product-surcharge')
    
    if (productSurchargeElement) {
      if (surcharge.value > 0) {
        productSurchargeElement.textContent = `Phụ phí: ${formatPrice(surcharge.value)}`
        productSurchargeElement.style.display = 'block'
      } else {
        productSurchargeElement.style.display = 'none'
      }
    }
  }
  
  const findAllLaceMeshes = () => {
    if (!model) return []
    
    const laceMeshes = []
    model.traverse((node) => {
      if (node.isMesh && 
         (node.name === 'Lace' || 
          node.name.toLowerCase().includes('lace') || 
          node.name.toLowerCase().includes('shoelace') || 
          node.name.toLowerCase().includes('string') ||
          node.name.toLowerCase().includes('cord'))) {
        laceMeshes.push(node)
      }
    })
    
    console.log(`Tìm thấy ${laceMeshes.length} mesh dây giày:`, 
      laceMeshes.map(mesh => ({ name: mesh.name, uuid: mesh.uuid })))
    
    return laceMeshes
  }
  
  // Xử lý khi thay đổi nhà sản xuất
  const handleManufacturerChange = () => {
    // Lấy thông tin nhà sản xuất hiện tại
    const manufacturer = currentManufacturer.value
    
    // Tính lại phụ phí với nhà sản xuất mới
    calculateSurcharge()
    
    // Khi thay đổi nhà sản xuất, có thể cần thay đổi mô hình 3D
    loadModelForManufacturer(manufacturer.id)
  }
  
  // Tải mô hình 3D tương ứng với nhà sản xuất
  const loadModelForManufacturer = (manufacturerId) => {
    const manufacturer = manufacturers.find(m => m.id === manufacturerId) || manufacturers[0]
    const dracoLoader = new DRACOLoader()
    dracoLoader.setDecoderPath('https://www.gstatic.com/draco/versioned/decoders/1.5.5/')
    const loader = new GLTFLoader()
    loader.setDRACOLoader(dracoLoader)

    // Ưu tiên model3DUrl nếu có
    const modelPath = model3DUrl.value || manufacturer.modelPath
    loader.load(
      modelPath,
      (gltf) => onModelLoaded(gltf),
      (xhr) => null,
      (error) => alert('Lỗi khi tải mô hình 3D')
    )
  }
  
  // Hàm hiển thị ảnh trước đó
  const showPreviousImage = () => {
    if (currentImageIndex.value > 0) {
      currentImageIndex.value--;
      const prevImage = uploadedImageHistory[currentImageIndex.value];
      selectedImage.value = prevImage.file;
      selectedImageName.value = prevImage.name;
      previewImageUrl.value = prevImage.imageUrl;
    }
  }
  
  // Hàm hiển thị ảnh tiếp theo
  const showNextImage = () => {
    if (currentImageIndex.value < uploadedImageHistory.length - 1) {
      currentImageIndex.value++;
      const nextImage = uploadedImageHistory[currentImageIndex.value];
      selectedImage.value = nextImage.file;
      selectedImageName.value = nextImage.name;
      previewImageUrl.value = nextImage.imageUrl;
    }
  }
  
  const goToAIPage = () => {
    router.push('/ai')
  }
  
  const aiPrompt = ref('')
  const generatedAIImage = ref(null)
  const aiError = ref(null)
  const isGenerating = ref(false)
  
  const moveToImageSection = () => {
    if (!generatedAIImage.value) return;
    
    try {
      // Thêm ảnh AI vào uploadedImageHistory
      uploadedImageHistory.push(generatedAIImage.value);
      
      // Cập nhật currentImageIndex để hiển thị ảnh mới nhất
      currentImageIndex.value = uploadedImageHistory.length - 1;
      
      // Cập nhật selectedImage và previewImageUrl
      selectedImage.value = generatedAIImage.value;
      previewImageUrl.value = generatedAIImage.value;
      
      // Chuyển sang tab Hình ảnh
      activeTab.value = 'image';
      
      // Xóa ảnh AI sau khi đã chuyển
      generatedAIImage.value = null;
    } catch (error) {
      console.error('Lỗi khi chuyển ảnh:', error);
    }
  };
  
  const generateAIImage = async () => {
    if (!aiPrompt.value.trim()) {
      aiError.value = 'Vui lòng nhập prompt';
      return;
    }
  
    isGenerating.value = true;
    aiError.value = null;
  
    try {
      const formData = new FormData();
      formData.append('Prompt', aiPrompt.value);
      formData.append('OwnerId', localStorage.getItem('userId'));
      formData.append('Status', '0');
  
      const result = await aiService.generateImage(formData);
      console.log('API Response:', result);
  
      if (result.data && result.data.imageUrl) {
        // Thêm timestamp vào URL để tránh cache
        const timestamp = new Date().getTime();
        const separator = result.data.imageUrl.includes('?') ? '&' : '?';
        const imageUrl = `${result.data.imageUrl}${separator}t=${timestamp}`;
        generatedAIImage.value = imageUrl;
        
        // Chuyển sang tab hình ảnh và thêm vào lịch sử
        const file = dataURLtoFile(imageUrl, 'ai-generated.png');
        uploadedImageHistory.push({
          file: file,
          name: 'AI Generated Image',
          imageUrl: imageUrl
        });
        currentImageIndex.value = uploadedImageHistory.length - 1;
        
        // Cập nhật preview
        selectedImage.value = file;
        selectedImageName.value = 'AI Generated Image';
        previewImageUrl.value = imageUrl;
        
        // Chuyển tab
        activeTab.value = 'image';
      } else {
        throw new Error('Không nhận được URL ảnh từ server');
      }
    } catch (error) {
      console.error('Error generating image:', error);
      aiError.value = 'Có lỗi khi tạo ảnh. Vui lòng thử lại.';
    } finally {
      isGenerating.value = false;
    }
  };
  
  const removeAIImage = () => {
    generatedAIImage.value = null;
  };
  
  // Thêm computed property để theo dõi component được chọn
  const selectedComponent = computed(() => {
    const selected = components.find(comp => comp.isSelected);
    return selected ? selected.name : null;
  });
  
  const showConfirmModal = ref(false)
  const generatedImageUrl = ref('')
  
  const handleConfirmSwitchTab = () => {
    showConfirmModal.value = false
    activeTab.value = 'image'
    
    // Thêm hình ảnh vào lịch sử
    if (generatedImageUrl.value) {
      const file = dataURLtoFile(generatedImageUrl.value, 'ai-generated.png')
      uploadedImageHistory.push({
        file: file,
        name: 'AI Generated Image',
        imageUrl: generatedImageUrl.value
      })
      currentImageIndex.value = uploadedImageHistory.length - 1
      
      // Cập nhật preview
      selectedImage.value = file
      selectedImageName.value = 'AI Generated Image' 
      previewImageUrl.value = generatedImageUrl.value
    }
  }
  
  const dataURLtoFile = (dataurl, filename) => {
    const arr = dataurl.split(',')
    const mime = arr[0].match(/:(.*?);/)[1]
    const bstr = atob(arr[1])
    let n = bstr.length
    const u8arr = new Uint8Array(n)
    while(n--){
      u8arr[n] = bstr.charCodeAt(n)
    }
    return new File([u8arr], filename, {type:mime})
  }
  </script>
  <style scoped>
  .custom-detail-page {
    height: 100vh;
    display: flex;
    flex-direction: column;
    align-items: center;
    position: relative;
    background-color: #f9f9f9;
    overflow-y: auto;
  }
  
  /* Product-name container styles */
  .product-name-container {
    display: flex;
    align-items: center;
    gap: 10px;
    margin-bottom: 5px;
  }
  
  .edit-name-btn {
    background: linear-gradient(135deg, #17a2b8, #138496);
    color: white;
    border: none;
    border-radius: 4px;
    padding: 4px 8px;
    font-size: 0.8rem;
    display: inline-flex;
    align-items: center;
    cursor: pointer;
    transition: all 0.3s ease;
  }
  
  .edit-name-btn:hover {
    background: linear-gradient(135deg, #138496, #0f6674);
    transform: translateY(-2px);
    box-shadow: 0 4px 8px rgba(23, 162, 184, 0.3);
  }
  
  /* Modal sửa tên sản phẩm */
  .edit-name-modal {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: rgba(0, 0, 0, 0.7);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 9999;
    padding: 20px;
    animation: fadeIn 0.3s ease;
    backdrop-filter: blur(5px);
  }
  
  @keyframes fadeIn {
    from { opacity: 0; }
    to { opacity: 1; }
  }
  
  .edit-name-modal-content {
    background-color: white;
    border-radius: 16px;
    width: 90%;
    max-width: 500px;
    box-shadow: 0 10px 30px rgba(0, 0, 0, 0.25);
    animation: slideIn 0.3s ease;
    border: 1px solid rgba(255, 255, 255, 0.1);
  }
  
  @keyframes slideIn {
    from { transform: translateY(-50px); opacity: 0; }
    to { transform: translateY(0); opacity: 1; }
  }
  
  .edit-name-modal-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 15px 20px;
    border-bottom: 1px solid #eee;
    background: linear-gradient(to right, #f9f9f9, #ffffff);
  }
  
  .edit-name-modal-header h3 {
    margin: 0;
    font-size: 1.4rem;
    font-weight: 700;
    color: #333;
  }
  
  .edit-name-modal-body {
    padding: 20px;
  }
  
  .form-group {
    display: flex;
    flex-direction: column;
    gap: 8px;
    margin-bottom: 15px;
  }
  
  .form-group label {
    font-weight: 600;
    color: #555;
    font-size: 0.95rem;
  }
  
  .form-control {
    padding: 10px 12px;
    border: 1px solid #ddd;
    border-radius: 6px;
    font-size: 1rem;
    transition: border-color 0.2s ease, box-shadow 0.2s ease;
  }
  
  .form-control:focus {
    border-color: #007bff;
    box-shadow: 0 0 0 3px rgba(0, 123, 255, 0.15);
    outline: none;
  }
  
  .edit-name-modal-actions {
    display: flex;
    justify-content: flex-end;
    gap: 15px;
    margin-top: 20px;
  }
  
  .edit-name-modal-actions .btn {
    min-width: 100px;
    border-radius: 5px;
    padding: 8px 16px;
    font-weight: 600;
    cursor: pointer;
    transition: all 0.3s ease;
    border: none;
  }
  
  .edit-name-modal-actions .btn-primary {
    background: linear-gradient(135deg, #007bff, #0056b3);
    color: white;
  }
  
  .edit-name-modal-actions .btn-primary:hover {
    background: linear-gradient(135deg, #0069d9, #004494);
    box-shadow: 0 5px 15px rgba(0, 123, 255, 0.3);
    transform: translateY(-3px);
  }
  
  .edit-name-modal-actions .btn-secondary {
    background: linear-gradient(135deg, #6c757d, #5a6268);
    color: white;
  }
  
  .edit-name-modal-actions .btn-secondary:hover {
    background: linear-gradient(135deg, #5a6268, #4e555b);
    box-shadow: 0 5px 15px rgba(108, 117, 125, 0.3);
    transform: translateY(-3px);
  }
  
  /* Responsive styling */
  @media (max-width: 768px) {
    .product-name-container {
      flex-direction: column;
      align-items: flex-start;
    }
    
    .edit-name-btn {
      margin-top: 5px;
    }
  }
  
  /* 3D Model container */
  .model-container-wrapper {
    width: 100%;
    height: 50vh;
    margin-bottom: 20px;
    border-radius: 8px;
    background-color: #f0f0f0;
    box-shadow: inset 0 0 10px rgba(0,0,0,0.05);
    position: relative;
    overflow: hidden;
    transition: height 0.3s ease;
  }
  
  .model-container-wrapper.expanded {
    height: 65vh;
  }
  
  .canvas-resizer {
    position: absolute;
    right: 10px;
    bottom: 10px;
    z-index: 10;
  }
  
  .resize-handle {
    background-color: rgba(255, 255, 255, 0.8);
    border: 1px solid #ddd;
    width: 30px;
    height: 30px;
    border-radius: 50%;
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    transition: all 0.2s ease;
  }
  
  .resize-handle:hover {
    background-color: #fff;
    box-shadow: 0 4px 8px rgba(0,0,0,0.15);
  }
  
  .resize-icon {
    font-size: 14px;
    color: #333;
  }
  
  .model-container {
    width: 100%;
    height: 100%;
    transition: all 0.3s ease;
  }
  
  /* Customizer container layout */
  .customizer-container {
    display: flex;
    align-items: flex-start;
    padding: 20px;
    width: 100%;
    max-width: 1200px;
    margin: 20px auto;
    flex-wrap: wrap;
    gap: 20px;
  }
  
  .components-card,
  .customizer-card,
  .texture-controls-card,
  .image-preview-card {
    flex: 1;
    background: white;
    border-radius: 10px;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
    padding: 15px;
    min-width: 250px;
    margin-bottom: 20px;
  }
  
  .components-card {
    flex: 0 0 250px;
  }
  
  .image-preview-card {
    flex: 0 0 300px;
    order: 2;
    display: flex;
    justify-content: center;
    align-items: center;
    background-color: white;
    border-radius: 10px;
    box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
    padding: 15px;
    overflow: hidden;
  }
  
  .image-preview-container {
    position: relative;
    display: flex;
    flex-direction: column;
    align-items: center;
    width: 100%;
  }
  
  .image-preview-large {
    max-width: 100%;
    max-height: 250px;
    object-fit: contain;
    border-radius: 4px;
  }
  
  .image-name {
    font-size: 14px;
    color: #333;
    margin-top: 10px;
    text-align: center;
  }
  
  .image-preview-actions {
    display: flex;
    justify-content: space-between;
    margin-top: 10px;
  }
  
  .preview-action-button {
    background-color: white;
    border: 1px solid #ddd;
    border-radius: 5px;
    padding: 5px 10px;
    font-size: 14px;
    font-weight: 500;
    cursor: pointer;
    color: #333;
    transition: all 0.2s ease;
  }
  
  .preview-action-button:hover {
    background-color: #f5f5f5;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  }
  
  .customizer-card {
    flex: 1;
    order: 3;
    min-width: 300px;
  }
  
  .texture-controls-card {
    flex: 0 0 250px;
    order: 4;
  }
  
  /* Card styles */
  .components-card {
    background-color: white;
    border-radius: 10px;
    box-shadow: 0 4px 12px rgba(0,0,0,0.08);
    width: 100%;
    max-width: 170px;
  }
  
  .texture-controls-card {
    background-color: #1f2937;
    border-radius: 10px;
    padding: 10px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    color: white;
    margin: 0;
    max-width: 380px;
  }
  
  .customizer-card {
    background-color: white;
    border-radius: 10px;
    padding: 20px;
    box-shadow: 0 4px 12px rgba(0,0,0,0.08);
    max-width: 580px;
  }
  
  .card-title {
    font-size: 18px;
    font-weight: bold;
    margin-bottom: 15px;
    text-transform: uppercase;
    color: #333;
    border-bottom: 2px solid #f0f0f0;
    padding-bottom: 10px;
  }
  
  /* Tab styles */
  .customizer-tabs {
    display: flex;
    margin-bottom: 20px;
    border-bottom: 1px solid #eee;
  }
  
  .tab-button {
    padding: 8px 16px;
    border: none;
    background: none;
    font-size: 14px;
    cursor: pointer;
    color: #666;
    border-bottom: 2px solid transparent;
    transition: all 0.3s ease;
  }
  
  .tab-button.active {
    color: #4CAF50;
    border-bottom-color: #4CAF50;
  }
  
  .tab-button:hover {
    color: #4CAF50;
  }
  
  .button-group {
    display: flex;
    gap: 15px;
    margin-top: 15px;
  }
  
  .button-group-highlighted {
    margin-top: 20px;
    padding: 15px;
    background-color: #f9f9f9;
    border-radius: 8px;
    border: 2px dashed #ccc;
  }
  
  /* Primary button */
  .primary-button {
    background-color: #000;
    color: white;
  }
  
  .primary-button:hover {
    background-color: #333;
  }
  
  /* Apply and remove buttons */
  .apply-button, .remove-button {
    background-color: white;
    border: 1px solid #ddd;
    border-radius: 8px;
    padding: 15px 20px;
    font-size: 14px;
    font-weight: 600;
    cursor: pointer;
    color: #333;
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 8px;
    transition: all 0.2s ease;
    flex: 1;
  }
  
  .apply-button {
    background-color: #000;
    color: white;
    border-color: #000;
    font-size: 15px;
  }
  
  /* Action highlight button */
  .action-highlight {
    transform: scale(1.05);
    font-weight: 700;
    box-shadow: 0 4px 12px rgba(0,0,0,0.15);
    letter-spacing: 0.5px;
  }
  
  /* Swatches */
  .swatches-container {
    display: flex;
    justify-content: center;
    flex-wrap: wrap;
    align-items: center;
    gap: 15px;
  }
  
  .swatch-wrapper {
    display: flex;
    flex-direction: column;
    align-items: center;
  }
  
  .swatch {
    width: 35px;
    height: 35px;
    border-radius: 50%;
    cursor: pointer;
    border: 2px solid transparent;
    transition: all 0.2s ease;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  }
  
  .swatch-selected {
    border: 2px solid #000;
    transform: scale(1.1);
  }
  
  .swatch-label {
    margin-top: 5px;
    font-size: 12px;
    color: #000;
    text-align: center;
  }
  
  /* Components list */
  .components-list {
    list-style: none;
    padding: 0;
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    gap: 10px;
    margin-bottom: 20px;
  }
  
  .component-item {
    display: flex;
    align-items: center;
    font-size: 14px;
    color: #333;
    cursor: pointer;
    padding: 8px 15px;
    border-radius: 20px;
    transition: all 0.2s ease;
  }
  
  .component-item:hover {
    background-color: #f5f5f5;
    color: #000;
  }
  
  .component-selected {
    background-color: #f0f0f0;
    color: #000;
    font-weight: bold;
  }
  
  .component-dot {
    width: 10px;
    height: 10px;
    border-radius: 50%;
    background-color: #ccc;
    margin-right: 8px;
  }
  
  .component-selected .component-dot {
    background-color: #000;
  }
  
  /* Navigation and Label */
  .navigation-container {
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 15px;
    margin-bottom: 10px;
  }
  
  .nav-arrow {
    background: none;
    border: none;
    font-size: 20px;
    cursor: pointer;
    color: #000;
    width: 40px;
    height: 40px;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    transition: all 0.2s ease;
  }
  
  .nav-arrow:hover {
    background-color: #f0f0f0;
  }
  
  .nav-label {
    font-size: 16px;
    font-weight: bold;
    color: #333;
  }
  
  /* Product info */
  .product-info {
    position: absolute;
    top: 20px;
    left: 20px;
    text-align: left;
    z-index: 10;
  }
  
  .product-name {
    font-size: 20px;
    font-weight: bold;
    margin: 0;
    color: #000;
  }
  
  .product-price {
    font-size: 16px;
    font-weight: 600;
    margin: 5px 0 0 0;
    color: #444;
  }
  
  /* Action buttons */
  .action-buttons {
    position: absolute;
    top: 20px;
    right: 20px;
    display: flex;
    gap: 10px;
    z-index: 10;
  }
  
  .action-button {
    background-color: white;
    border: 1px solid #ddd;
    border-radius: 25px;
    padding: 10px 20px;
    font-size: 14px;
    font-weight: 500;
    cursor: pointer;
    color: #333;
    display: flex;
    align-items: center;
    gap: 8px;
    transition: all 0.25s ease;
    box-shadow: 0 2px 6px rgba(0,0,0,0.1);
  }
  
  .action-button:hover {
    background-color: #f5f5f5;
    box-shadow: 0 4px 12px rgba(0,0,0,0.15);
    transform: translateY(-2px);
  }
  
  .action-button:active {
    transform: translateY(1px);
    box-shadow: 0 1px 3px rgba(0,0,0,0.1);
  }
  
  /* Text customizer */
  .text-customizer {
    display: flex;
    flex-direction: column;
    gap: 10px;
    width: 100%;
  }
  
  .text-input-group {
    display: flex;
    align-items: center;
    gap: 10px;
    width: 100%;
  }
  
  .text-input {
    flex: 1; /* Để input chiếm phần còn lại của không gian */
    padding: 12px;
    border: 1px solid #ddd;
    border-radius: 8px;
    font-size: 14px;
    box-shadow: inset 0 1px 3px rgba(0,0,0,0.05);
    transition: all 0.2s ease;
  }
  
  .text-input:focus {
    border-color: #aaa;
    outline: none;
    box-shadow: inset 0 1px 3px rgba(0,0,0,0.05), 0 0 0 2px rgba(0,0,0,0.03);
  }
  
  .text-buttons {
    display: flex;
    gap: 10px;
  }
  
  .text-button {
    background-color: white;
    border: 1px solid #ddd;
    border-radius: 8px;
    padding: 9px 16px;
    white-space: nowrap;
    font-size: 14px;
    font-weight: 500;
    cursor: pointer;
    color: #333;
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 8px;
    transition: all 0.2s ease;
    box-shadow: 0 1px 3px rgba(0,0,0,0.05);
  }
  
  .text-button:hover {
    background-color: #f5f5f5;
    box-shadow: 0 2px 6px rgba(0,0,0,0.1);
    transform: translateY(-1px);
  }
  
  .apply-text {
    background-color: #000;
    color: white;
    border-color: #000;
  }
  
  .apply-text:hover {
    background-color: #333;
  }
  
  .remove-text:hover {
    background-color: #ffeeee;
    border-color: #ffcccc;
  }
  
  /* Image customizer */
  .image-customizer {
    display: flex;
    flex-direction: column;
    gap: 10px;
    width: 100%;
  }
  
  .image-input-wrapper {
    display: flex;
    align-items: center;
    gap: 10px;
    width: 100%;
    flex-wrap: wrap;
  }
  
  .image-input {
    display: none;
  }
  
  .upload-button {
    flex: 1;
    min-width: 150px;
    background-color: white;
    border: 1px solid #ddd;
    border-radius: 8px;
    padding: 12px;
    text-align: center;
    font-size: 14px;
    font-weight: 500;
    cursor: pointer;
    color: #333;
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 8px;
    transition: all 0.2s ease;
    box-shadow: 0 1px 3px rgba(0,0,0,0.05);
  }
  
  .upload-button:hover {
    background-color: #f5f5f5;
    box-shadow: 0 2px 6px rgba(0,0,0,0.1);
  }
  
  .image-buttons {
    display: flex;
    justify-content: center;
    gap: 10px;
  }
  
  .text-button {
    padding: 8px 20px;
    border-radius: 5px;
    font-size: 14px;
    font-weight: 500;
    cursor: pointer;
    transition: all 0.2s ease;
    border: none;
  }
  
  .apply-text {
    background-color: #3498db;
    color: white;
  }
  
  .apply-text:hover {
    background-color: #2980b9;
  }
  
  .remove-text {
    background-color: #e74c3c;
    color: white;
  }
  
  .remove-text:hover {
    background-color: #c0392b;
  }
  
  .apply-text:disabled {
    background-color: #95a5a6;
    cursor: not-allowed;
  }
  
  .file-name {
    font-size: 14px;
    color: #333;
    padding: 8px 0;
    text-align: center;
  }
  
  /* Responsive adjustments */
  @media (max-width: 768px) {
    .customizer-container {
      flex-direction: column;
      align-items: center;
    }
    
    .components-card, .customizer-card {
      max-width: 90%;
    }
    
    .product-info, .action-buttons {
      position: relative;
      top: 0;
      left: 0;
      right: 0;
      margin: 10px 0;
    }
    
    .action-buttons {
      justify-content: center;
    }
  }
  
  .button-group-highlighted {
    margin-top: 10px;
  }
  
  .input-hint {
    font-size: 12px;
    color: #666;
    margin-top: 8px;
    text-align: center;
    font-style: italic;
  }
  
  @media (max-width: 768px) {
    .text-input-group {
      flex-direction: column;
      align-items: stretch;
    }
    
    .text-buttons {
      margin-top: 10px;
      justify-content: space-between;
    }
    
    .text-button {
      flex: 1;
    }
  }
  
  .image-preview-container {
    margin-top: 15px;
    display: flex;
    flex-direction: column;
    align-items: center;
  }
  
  .image-preview {
    max-width: 100%;
    max-height: 150px;
    border-radius: 4px;
    object-fit: contain;
  }
  
  .file-name {
    font-size: 13px;
    color: #555;
    font-style: italic;
  }
  
  /* Capture modal */
  .capture-modal {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 9999; /* Tăng z-index lên cao hơn */
  }
  
  .capture-modal-content {
    background-color: white;
    border-radius: 10px;
    width: 90%;
    max-width: 800px;
    max-height: 90vh;
    overflow-y: auto;
    box-shadow: 0 5px 20px rgba(0, 0, 0, 0.2);
  }
  
  .capture-modal-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 15px 20px;
    border-bottom: 1px solid #eee;
  }
  
  .capture-modal-header h3 {
    margin: 0;
    font-size: 18px;
  }
  
  .close-button {
    background: none;
    border: none;
    font-size: 24px;
    cursor: pointer;
    color: #555;
  }
  
  .capture-modal-body {
    padding: 20px;
  }
  
  .preview-angles {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(120px, 1fr));
    gap: 15px;
    margin-bottom: 20px;
  }
  
  .preview-angle {
    cursor: pointer;
    border: 2px solid transparent;
    border-radius: 8px;
    overflow: hidden;
    transition: all 0.2s ease;
  }
  
  .preview-angle.selected {
    border-color: #000;
    transform: scale(1.05);
  }
  
  .angle-preview {
    height: 100px;
    background-color: #f0f0f0;
    display: flex;
    align-items: center;
    justify-content: center;
    overflow: hidden;
  }
  
  .angle-preview img {
    width: 100%;
    height: 100%;
    object-fit: cover;
  }
  
  .placeholder {
    width: 100%;
    height: 100%;
    display: flex;
    align-items: center;
    justify-content: center;
    color: #888;
    font-size: 12px;
  }
  
  .angle-name {
    padding: 8px;
    font-size: 12px;
    text-align: center;
    background-color: #f9f9f9;
    border-top: 1px solid #eee;
  }
  
  .download-controls {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 15px;
    margin-bottom: 20px;
  }
  
  .camera-button {
    background-color: #333;
    color: white;
    border: none;
    border-radius: 25px;
    padding: 10px 20px;
    font-size: 14px;
    font-weight: 500;
    cursor: pointer;
    display: flex;
    align-items: center;
    gap: 8px;
    transition: all 0.2s ease;
  }
  
  .camera-button:hover {
    background-color: #555;
  }
  
  .rotate-button {
    width: 40px;
    height: 40px;
    background-color: white;
    border: 1px solid #ddd;
    border-radius: 8px;
    cursor: pointer;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 18px;
    transition: all 0.2s ease;
  }
  
  .rotate-button:hover {
    background-color: #f0f0f0;
  }
  
  .rotate-button:nth-child(1) {
    grid-column: 1;
    grid-row: 2;
  }
  
  .rotate-button:nth-child(2) {
    grid-column: 3;
    grid-row: 2;
  }
  
  .rotate-button:nth-child(3) {
    grid-column: 2;
    grid-row: 1;
  }
  
  .rotate-button:nth-child(4) {
    grid-column: 2;
    grid-row: 3;
  }
  
  .capture-actions {
    display: flex;
    justify-content: center;
    gap: 15px;
    margin-top: 20px;
  }
  
  /* Mới thêm: CSS cho product-summary và complete-actions */
  .product-summary {
    background-color: #f9f9f9;
    border-radius: 8px;
    padding: 15px;
    margin: 20px 0;
    border: 1px solid #eee;
  }
  
  .product-summary h4 {
    margin-top: 0;
    margin-bottom: 15px;
    font-size: 16px;
    color: #333;
    border-bottom: 1px solid #ddd;
    padding-bottom: 8px;
  }
  
  .summary-info p {
    margin: 8px 0;
    font-size: 14px;
    color: #444;
  }
  
  .complete-actions {
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    gap: 15px;
    margin-top: 20px;
  }
  
  .complete-actions .action-button {
    flex: 1;
    justify-content: center;
    min-width: 150px;
    font-weight: 600;
  }
  
  .complete-actions .primary-button {
    flex: 2;
  }
  
  @media (max-width: 480px) {
    .complete-actions {
      flex-direction: column;
    }
    
    .complete-actions .action-button {
      width: 100%;
    }
  }
  
  /* Thêm công cụ điều chỉnh texture */
  .texture-controls-card {
    background-color: #1f2937;
    border-radius: 10px;
    padding: 10px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    color: white;
    margin: 0;
    max-width: 380px;
  }
  
  .texture-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 10px;
  }
  
  .texture-controls {
    margin-top: 5px;
  }
  
  .texture-controls-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 8px;
  }
  
  .texture-control {
    display: flex;
    flex-direction: column;
    gap: 3px;
    font-size: 12px;
  }
  
  .control-label {
    font-family: monospace;
    font-size: 12px;
    color: #a3a3a3;
  }
  
  .control-slider {
    width: 100%;
    position: relative;
  }
  
  .slider {
    -webkit-appearance: none;
    width: 100%;
    height: 4px;
    border-radius: 10px;
    background: #4b5563;
    outline: none;
  }
  
  .slider::-webkit-slider-thumb {
    -webkit-appearance: none;
    appearance: none;
    width: 12px;
    height: 12px;
    border-radius: 50%;
    background: #3b82f6;
    cursor: pointer;
    box-shadow: 0 0 0 2px rgba(59, 130, 246, 0.3);
  }
  
  .slider::-moz-range-thumb {
    width: 12px;
    height: 12px;
    border-radius: 50%;
    background: #3b82f6;
    cursor: pointer;
    box-shadow: 0 0 0 2px rgba(59, 130, 246, 0.3);
    border: none;
  }
  
  .control-value {
    text-align: center;
    font-family: monospace;
    background-color: #374151;
    padding: 2px 5px;
    border-radius: 4px;
    font-size: 11px;
  }
  
  .texture-controls-card .card-title {
    color: white;
    border-bottom: none;
    font-size: 13px;
    padding-bottom: 0;
    margin-bottom: 0;
    margin-top: 0;
  }
  
  @media (max-width: 600px) {
    .texture-controls-grid {
      grid-template-columns: 1fr;
    }
  }
  
  @media (min-width: 601px) and (max-width: 900px) {
    .texture-controls-grid {
      grid-template-columns: repeat(2, 1fr);
    }
  }
  
  /* Dropdown styles - Thêm phần CSS cho dropdown */
  .components-dropdown-container {
    margin: 15px 0;
  }
  
  .components-dropdown {
    width: 100%;
    padding: 12px 15px;
    border: 1px solid #ddd;
    border-radius: 8px;
    background-color: white;
    font-size: 16px;
    color: #333;
    appearance: none;
    background-image: url("data:image/svg+xml;charset=UTF-8,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='none' stroke='currentColor' stroke-width='2' stroke-linecap='round' stroke-linejoin='round'%3e%3cpolyline points='6 9 12 15 18 9'%3e%3c/polyline%3e%3c/svg%3e");
    background-repeat: no-repeat;
    background-position: right 15px center;
    background-size: 15px;
    cursor: pointer;
    transition: all 0.2s ease;
  }
  
  .components-dropdown:hover {
    border-color: #aaa;
  }
  
  .components-dropdown:focus {
    outline: none;
    border-color: #000;
    box-shadow: 0 0 0 2px rgba(0,0,0,0.05);
  }
  
  .selected-component-info {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-top: 10px;
    padding: 10px 15px;
    background-color: #f9f9f9;
    border-radius: 8px;
    font-size: 14px;
  }
  
  .component-name {
    font-weight: bold;
    color: #000;
  }
  
  /* Color Picker Styles */
  .color-picker-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    margin-top: 15px;
  }
  
  .color-picker-wrapper {
    display: flex;
    align-items: center;
  }
  
  .color-picker-input {
    width: 50px;
    height: 30px;
    margin-right: 10px;
  }
  
  .apply-custom-color-btn {
    background-color: #000;
    color: white;
    border: none;
    border-radius: 5px;
    padding: 3px 10px;
    font-size: 14px;
    cursor: pointer;
  }
  
  .manufacturer-selector {
    margin-top: 10px;
  }
  
  .surcharge-modal {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 9999; /* Tăng z-index lên cao hơn */
  }
  
  .surcharge-modal-content {
    background-color: white;
    border-radius: 10px;
    width: 90%;
    max-width: 800px;
    max-height: 90vh;
    overflow-y: auto;
    box-shadow: 0 5px 20px rgba(0, 0, 0, 0.2);
  }
  
  .surcharge-modal-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 15px 20px;
    border-bottom: 1px solid #eee;
  }
  
  .surcharge-modal-header h3 {
    margin: 0;
    font-size: 18px;
  }
  
  .close-button {
    background: none;
    border: none;
    font-size: 24px;
    cursor: pointer;
    color: #555;
  }
  
  .surcharge-modal-body {
    padding: 20px;
  }
  
  .surcharge-table {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 10px;
    margin-top: 15px;
    background-color: #f8f9fa;
    border-radius: 8px;
    padding: 15px;
  }
  
  .surcharge-item {
    display: flex;
    justify-content: space-between;
    padding: 5px 10px;
    border-bottom: 1px solid #eee;
  }
  
  .surcharge-item:last-child {
    border-bottom: none;
  }
  
  .surcharge-item span:last-child {
    font-weight: bold;
    color: #e74c3c;
  }
  
  .component-surcharge-details {
    margin-top: 15px;
  }
  
  .surcharge-detail-table {
    border-radius: 8px;
    overflow: hidden;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    border: 1px solid #eaeaea;
    width: 100%;
  }
  
  .surcharge-detail-header {
    display: grid;
    grid-template-columns: 1fr 1fr 1fr;
    background-color: #007bff;
    color: white;
    font-weight: 600;
    text-align: center;
  }
  
  .surcharge-detail-row {
    display: grid;
    grid-template-columns: 1fr 1fr 1fr;
    border-bottom: 1px solid #eee;
    background-color: white;
    transition: background-color 0.2s ease;
  }
  
  .surcharge-detail-row:nth-child(even) {
    background-color: #f9f9f9;
  }
  
  .surcharge-detail-row:hover {
    background-color: #f0f8ff;
  }
  
  .surcharge-detail-row:last-child {
    border-bottom: none;
  }
  
  .detail-col {
    padding: 12px 15px;
    display: flex;
    align-items: center;
    justify-content: center;
    text-align: center;
  }
  
  .detail-col:first-child {
    font-weight: 500;
  }
  
  .detail-col:nth-child(2), .detail-col:nth-child(3) {
    color: #e74c3c;
    font-weight: bold;
  }
  
  .surcharge-note {
    background-color: #fff8e6;
    border-left: 4px solid #ffc107;
    padding: 15px;
    margin-top: 20px;
    border-radius: 0 8px 8px 0;
    box-shadow: 0 1px 4px rgba(0,0,0,0.05);
  }
  
  .surcharge-note p {
    margin: 0;
    color: #6c4a00;
    font-size: 0.95rem;
    line-height: 1.5;
  }
  
  .surcharge-modal-header h3 {
    font-size: 1.3rem;
    font-weight: 600;
    color: #333;
    margin: 0;
    position: relative;
    padding-bottom: 5px;
  }
  
  .surcharge-modal-header h3::after {
    content: '';
    position: absolute;
    bottom: 0;
    left: 0;
    width: 50px;
    height: 3px;
  }
  
  .mt-4 {
    margin-top: 1.5rem;
  }
  
  /* Manufacturer selector */
  .manufacturer-selector {
    margin-top: 15px;
    background-color: rgba(255, 255, 255, 0.9);
    border-radius: 8px;
    padding: 10px;
    box-shadow: 0 2px 6px rgba(0,0,0,0.1);
    display: flex;
    align-items: center;
    gap: 10px;
    max-width: 250px;
  }
  
  .manufacturer-selector label {
    font-weight: 600;
    font-size: 14px;
    color: #333;
    white-space: nowrap;
  }
  
  .manufacturer-selector select {
    padding: 8px 12px;
    border: 1px solid #ddd;
    border-radius: 6px;
    font-size: 14px;
    background-color: white;
    cursor: pointer;
    appearance: none;
    background-image: url("data:image/svg+xml;charset=UTF-8,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='none' stroke='currentColor' stroke-width='2' stroke-linecap='round' stroke-linejoin='round'%3e%3cpolyline points='6 9 12 15 18 9'%3e%3c/polyline%3e%3c/svg%3e");
    background-repeat: no-repeat;
    background-position: right 10px center;
    background-size: 12px;
    padding-right: 30px;
    flex: 1;
  }
  
  .manufacturer-selector select:hover {
    border-color: #aaa;
  }
  
  .manufacturer-selector select:focus {
    outline: none;
    border-color: #007bff;
    box-shadow: 0 0 0 3px rgba(0,123,255,0.15);
  }
  
  /* Surcharge Modal */
  .surcharge-modal {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 9999;
  }
  
  .surcharge-modal-content {
    background-color: white;
    border-radius: 10px;
    width: 90%;
    max-width: 500px;
    max-height: 90vh;
    overflow-y: auto;
    box-shadow: 0 5px 20px rgba(0, 0, 0, 0.2);
  }
  
  .surcharge-modal-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 15px 20px;
    border-bottom: 1px solid #eee;
    background-color: #f8f9fa;
  }
  
  .surcharge-modal-header h3 {
    margin: 0;
    font-size: 18px;
    color: #333;
  }
  
  .surcharge-modal-body {
    padding: 20px;
  }
  
  .surcharge-table {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 10px;
    margin-top: 15px;
    background-color: #f8f9fa;
    border-radius: 8px;
    padding: 15px;
  }
  
  .surcharge-item {
    display: flex;
    justify-content: space-between;
    padding: 5px 10px;
    border-bottom: 1px solid #eee;
  }
  
  .surcharge-item:last-child {
    border-bottom: none;
  }
  
  .surcharge-item span:last-child {
    font-weight: bold;
    color: #e74c3c;
  }
  
  @media (max-width: 768px) {
    .surcharge-table {
      grid-template-columns: 1fr;
    }
    
    .manufacturer-selector {
      flex-direction: column;
      align-items: flex-start;
    }
    
    .manufacturer-selector select {
      width: 100%;
    }
  }
  
  .image-preview-card {
    background-color: white;
    border-radius: 10px;
    padding: 15px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    max-width: 300px;
  }
  
  .image-preview-wrapper {
    display: flex;
    flex-direction: column;
    align-items: center;
  }
  
  .image-preview-large {
    max-width: 100%;
    max-height: 200px;
    border-radius: 4px;
    object-fit: contain;
  }
  
  .image-name {
    font-size: 14px;
    color: #333;
    margin-top: 10px;
    text-align: center;
  }
  
  .image-preview-actions {
    display: flex;
    justify-content: space-between;
    margin-top: 10px;
  }
  
  .preview-action-button {
    background-color: white;
    border: 1px solid #ddd;
    border-radius: 5px;
    padding: 5px 10px;
    font-size: 14px;
    font-weight: 500;
    cursor: pointer;
    color: #333;
    transition: all 0.2s ease;
  }
  
  .preview-action-button:hover {
    background-color: #f5f5f5;
    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  }
  
  .back-image-button, .next-image-button {
    background-color: rgba(255, 255, 255, 0.8);
    border: none;
    border-radius: 50%;
    width: 32px;
    height: 32px;
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    transition: all 0.2s ease;
    font-size: 18px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.2);
  }
  
  .back-image-button:hover, .next-image-button:hover {
    background-color: rgba(255, 255, 255, 1);
    transform: scale(1.1);
  }
  
  .image-navigation-buttons {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0 10px;
    z-index: 10;
    pointer-events: none;
  }
  
  .image-navigation-buttons button {
    pointer-events: auto;
  }
  
  .next-image-button {
    margin-left: auto;
  }
  
  .image-counter {
    position: absolute;
    bottom: 10px;
    right: 10px;
    background-color: rgba(0, 0, 0, 0.6);
    color: white;
    padding: 2px 8px;
    border-radius: 10px;
    font-size: 12px;
  }
  
  .ai-form {
    display: flex;
    flex-direction: column;
    gap: 15px;
    padding: 15px;
  }
  
  .ai-form .form-group {
    display: flex;
    flex-direction: column;
    gap: 8px;
  }
  
  .ai-form .input-text {
    padding: 8px 12px;
    border: 1px solid #ddd;
    border-radius: 4px;
    font-size: 14px;
  }
  
  .ai-form .submit-button {
    background-color: #007bff;
    color: white;
    border: none;
    border-radius: 4px;
    padding: 8px 16px;
    cursor: pointer;
    font-size: 14px;
  }
  
  .ai-form .submit-button:disabled {
    background-color: #ccc;
    cursor: not-allowed;
  }
  
  /* Điều chỉnh kích thước ảnh được gen */
  .result-section {
    position: absolute;
    right: 20px;
    top: 50%;
    transform: translateY(-50%);
    width: 300px;
    z-index: 10;
    background: rgba(255, 255, 255, 0.95);
    border-radius: 8px;
    padding: 10px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  }
  
  .image-container {
    position: relative;
    width: 100%;
    margin: 0 auto;
  }
  
  .generated-image {
    width: 100%;
    height: auto;
    border-radius: 4px;
    display: block;
    margin: 0 auto;
    max-height: 300px;
    object-fit: contain;
  }
  
  .image-buttons {
    display: flex;
    justify-content: center;
    gap: 10px;
    margin-top: 10px;
  }
  
  .text-button {
    padding: 8px 16px;
    border-radius: 4px;
    border: none;
    cursor: pointer;
    font-weight: 500;
    transition: all 0.3s ease;
  }
  
  .apply-text {
    background-color: #4CAF50;
    color: white;
  }
  
  .remove-text {
    background-color: #f44336;
    color: white;
  }
  
  .text-button:hover {
    opacity: 0.9;
    transform: translateY(-1px);
  }
  
  #canvas-container {
    position: relative;
    width: 100%;
    height: 100%;
  }
  
  #canvas {
    width: 100%;
    height: 100%;
  }
  
  @media (max-width: 768px) {
    .result-section {
      position: fixed;
      bottom: 20px;
      right: 20px;
      top: auto;
      transform: none;
      width: calc(100% - 40px);
      max-width: 300px;
    }
  }
  
  .ai-generate-section {
    padding: 20px;
    background: white;
    border-radius: 8px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  }
  
  .input-section {
    display: flex;
    gap: 10px;
    margin-bottom: 20px;
  }
  
  .prompt-input {
    flex: 1;
    padding: 8px 12px;
    border: 1px solid #ddd;
    border-radius: 4px;
    font-size: 14px;
  }
  
  .generate-button {
    padding: 8px 16px;
    background: #007bff;
    color: white;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    transition: background 0.3s;
  }
  
  .generate-button:disabled {
    background: #ccc;
    cursor: not-allowed;
  }
  
  .generate-button:not(:disabled):hover {
    background: #0056b3;
  }
  
  .error-message {
    color: #dc3545;
    margin-top: 8px;
    font-size: 14px;
  }
  
  .result-section {
    margin-top: 20px;
  }
  
  .image-container {
    position: relative;
    width: 100%;
    max-width: 512px;
    margin: 0 auto;
    background: rgba(255, 255, 255, 0.1);
    border-radius: 8px;
    padding: 10px;
  }
  
  .generated-image {
    width: 100%;
    height: auto;
    border-radius: 4px;
    display: block;
    margin: 0 auto;
    max-height: 512px;
    object-fit: contain;
  }
  
  .image-buttons {
    display: flex;
    justify-content: center;
    gap: 10px;
    margin-top: 10px;
  }
  
  .text-button {
    padding: 8px 16px;
    border-radius: 4px;
    border: none;
    cursor: pointer;
    font-weight: 500;
    transition: all 0.3s ease;
  }
  
  .apply-text {
    background-color: #4CAF50;
    color: white;
  }
  
  .remove-text {
    background-color: #f44336;
    color: white;
  }
  
  .text-button:hover {
    opacity: 0.9;
    transform: translateY(-1px);
  }
  </style>