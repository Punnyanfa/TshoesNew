<template>
  <div class="custom-detail-page">
    <!-- Thông tin sản phẩm ở góc trái trên -->
    <div class="product-info">
      <h2 class="product-name">Adidas Running Shoes</h2>
      <p class="product-price">2.500.000 ₫</p>
    </div>
    
    <!-- Các nút chức năng ở góc phải trên -->
    <div class="action-buttons" style="display: flex; gap: 10px; justify-content: flex-end;">
      <button class="action-button" @click="openCaptureModal">
         Tải ảnh
      </button>
      <button class="action-button primary-button" @click="handleDone">
      Hoàn thành
      </button>
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
            <button class="action-button" @click="downloadSelectedAngle">
               Tải ảnh đã chọn
            </button>
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
              <p><strong>Tên sản phẩm:</strong> Adidas Running Shoes (Tùy chỉnh)</p>
              <p><strong>Giá:</strong> 2.500.000 ₫</p>
            </div>
          </div>
          
          <div class="complete-actions">
            <button class="action-button" @click="saveAsDraft">
               Lưu nháp
            </button>
            <button class="action-button primary-button" @click="addToCart">
               Thêm vào giỏ hàng
            </button>
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
        <!-- Thay thế components-list bằng dropdown -->
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
        
        <!-- Hiển thị thông tin component đã chọn -->
        <div class="selected-component-info">
          <div class="component-name">{{ components[selectedComponentIndex].name }}</div>
          <div class="component-index">{{ selectedComponentIndex + 1 }}/{{ components.length }}</div>
        </div>
      </div>
      
      <!-- Tùy chỉnh màu và texture ở giữa -->
      <div class="customizer-card">
        <h3 class="card-title">TÙY CHỈNH</h3>
        
        <!-- Tabs for different customization options -->
        <div class="customizer-tabs">
          <button 
            :class="{'tab-button': true, 'active': activeTab === 'color'}" 
            @click="activeTab = 'color'"
          >
            Màu nền
          </button>
          <button 
            :class="{'tab-button': true, 'active': activeTab === 'text'}" 
            @click="activeTab = 'text'"
          >
            Văn bản
          </button>
          <button 
            :class="{'tab-button': true, 'active': activeTab === 'image'}" 
            @click="activeTab = 'image'"
          >
            Hình ảnh
          </button>
        </div>
        
        <!-- Color swatches -->
        <div v-if="activeTab === 'color'" class="tab-content">
          <!-- Thêm Color Picker -->
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
        
        <!-- Text input -->
        <div v-if="activeTab === 'text'" class="tab-content">
          <div class="text-customizer">
            <div class="text-input-group">
              <input 
                type="text" 
                v-model="customText" 
                placeholder="Nhập văn bản tùy chỉnh"
                class="text-input"
              />
              <div class="text-buttons">
                <button class="text-button apply-text" @click="applyTextToMesh" title="Áp dụng văn bản vào phần đã chọn">
                   Áp dụng
                </button>
                <button class="text-button remove-text" @click="removeTextFromMesh" title="Xóa văn bản khỏi phần đã chọn">
                   Xóa
                </button>
              </div>
            </div>
            
            <!-- Thêm pallete màu chữ -->
            <!-- Đã xóa phần màu chữ -->
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
                <button class="text-button remove-text" @click="removeTextFromMesh" title="Xóa ảnh khỏi phần đã chọn">
                   Xóa
                </button>
              </div>
            </div>
            
            <!-- Hiển thị hình ảnh đã chọn -->
            <div v-if="selectedImage" class="image-preview-container">
              <img :src="previewImageUrl" alt="Preview" class="image-preview" />
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
</template>

<script setup>
import { ref, reactive, onMounted, onBeforeUnmount, watch } from 'vue'
import * as THREE from 'three'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader'
import { DRACOLoader } from 'three/examples/jsm/loaders/DRACOLoader'

// Container tham chiếu
const container = ref(null)

// State cho canvas size
const isCanvasExpanded = ref(false)

// State cho modal chụp ảnh
const showCaptureModal = ref(false)
// State cho modal hoàn thành
const showCompleteModal = ref(false)
const selectedAngleIndex = ref(0)
const captureAngles = reactive([
  //{ name: 'Mặt trước', preview: null, position: { x: 0, y: 0, z: 8 } },
  { name: 'Mặt sau', preview: null, position: { x: -6, y: 2, z: -6 } },
  { name: 'Bên trái', preview: null, position: { x: -4, y: 0, z: 6 } },
  { name: 'Bên phải', preview: null, position: { x: 4, y: 0, z: -6 } },
  { name: 'Góc nhìn trên', preview: null, position: { x: 4, y: 8, z: 4 } },
  { name: 'Góc nhìn dưới', preview: null, position: { x: -4, y: -12, z: 4 } }
])

// Lưu trữ vị trí camera hiện tại để khôi phục sau khi chụp
let originalCameraPosition = null

// Mở modal chụp ảnh
const openCaptureModal = () => {
  console.log('Mở modal chụp ảnh')
  // Lưu vị trí camera hiện tại
  originalCameraPosition = {
    position: camera ? new THREE.Vector3().copy(camera.position) : null,
    rotation: controls ? controls.target.clone() : null
  }
  
  showCaptureModal.value = true
  
  // Tự động chụp ảnh khi mở modal
  captureAllAngles()
}

// Chọn góc chụp
const selectAngle = (index) => {
  selectedAngleIndex.value = index
  
  // Di chuyển camera đến vị trí của góc nhìn đã chọn
  if (camera && controls) {
    const anglePosition = captureAngles[index].position
    
    // Tạo animation di chuyển camera đến vị trí mới
    const startPosition = camera.position.clone()
    const targetPosition = new THREE.Vector3(anglePosition.x, anglePosition.y, anglePosition.z)
    
    // Reset target của controls về trung tâm
    controls.target.set(0, 0, 0)
    
    // Animation di chuyển camera
    const duration = 500 // milliseconds
    const startTime = Date.now()
    
    function animateCamera() {
      const now = Date.now()
      const elapsed = now - startTime
      const progress = Math.min(elapsed / duration, 1)
      
      // Sử dụng easing function để làm mượt chuyển động
      const easeProgress = progress < 0.5 ? 2 * progress * progress : 1 - Math.pow(-2 * progress + 2, 2) / 2
      
      // Di chuyển camera theo nội suy
      camera.position.lerpVectors(startPosition, targetPosition, easeProgress)
      
      // Cập nhật controls
      controls.update()
      
      if (progress < 1) {
        requestAnimationFrame(animateCamera)
      } else {
        // Tự động chụp ảnh sau khi camera đã di chuyển đến vị trí
        captureCurrentAngle()
      }
    }
    
    animateCamera()
  }
}

// Xoay camera
const rotateCamera = (direction) => {
  if (!camera || !controls) return
  
  const rotationSpeed = 0.3
  
  switch(direction) {
    case 'left':
      controls.rotateLeft(rotationSpeed)
      break;
    case 'right':
      controls.rotateLeft(-rotationSpeed)
      break;
    case 'up':
      controls.rotateUp(rotationSpeed)
      break;
    case 'down':
      controls.rotateUp(-rotationSpeed)
      break;
  }
  
  controls.update()
}

// Chụp ảnh góc hiện tại
const captureCurrentAngle = () => {
  if (!renderer) return
  
  // Render scene một lần nữa để đảm bảo ảnh mới nhất
  renderer.render(scene, camera)
  
  // Lấy dữ liệu hình ảnh từ canvas
  const imageData = renderer.domElement.toDataURL('image/png')
  
  // Lưu ảnh vào góc nhìn đã chọn
  captureAngles[selectedAngleIndex.value].preview = imageData
}

// Chụp ảnh tất cả các góc
const captureAllAngles = async () => {
  // Lưu vị trí camera ban đầu
  const initialPosition = camera.position.clone()
  const initialTarget = controls.target.clone()
  
  // Đặt lại target về trung tâm
  controls.target.set(0, 0, 0)
  
  // Duyệt qua từng góc và chụp
  for (let i = 0; i < captureAngles.length; i++) {
    // Chọn góc hiện tại
    selectedAngleIndex.value = i
    
    // Di chuyển camera đến góc đó
    const anglePosition = captureAngles[i].position
    camera.position.set(anglePosition.x, anglePosition.y, anglePosition.z)
    
    // Cập nhật controls
    controls.update()
    
    // Đợi một chút để camera ổn định
    await new Promise(resolve => setTimeout(resolve, 300))
    
    // Render scene và chụp ảnh
    renderer.render(scene, camera)
    const imageData = renderer.domElement.toDataURL('image/png')
    captureAngles[i].preview = imageData
  }
  
  // Khôi phục vị trí camera ban đầu
  camera.position.copy(initialPosition)
  controls.target.copy(initialTarget)
  controls.update()
  
  // Chọn lại góc đầu tiên
  selectedAngleIndex.value = 0
}

// Tải xuống tất cả các ảnh đã chụp
const downloadAllAngles = () => {
  // Kiểm tra xem có ảnh nào chưa chụp không
  const missingAngles = captureAngles.filter(angle => !angle.preview)
  
  // Nếu có góc chưa chụp, chụp tất cả trước
  if (missingAngles.length > 0) {
    captureAllAngles().then(() => {
      // Sau khi chụp xong, tải từng ảnh
      downloadImages()
    })
  } else {
    // Nếu đã có đủ ảnh, tải xuống ngay
    downloadImages()
  }
  
  function downloadImages() {
    captureAngles.forEach((angle, index) => {
      const link = document.createElement('a')
      link.href = angle.preview
      link.download = `adidas-custom-shoe-${angle.name}.png`
      
      // Thêm delay để tránh trình duyệt chặn nhiều lần tải xuống
      setTimeout(() => {
        document.body.appendChild(link)
        link.click()
        document.body.removeChild(link)
        console.log(`Đã tải ảnh từ góc nhìn: ${angle.name}`)
      }, index * 500)
    })
  }
}

// Tải ảnh đã chọn
const downloadSelectedAngle = () => {
  const selectedAngle = captureAngles[selectedAngleIndex.value]
  
  // Nếu chưa có ảnh, chụp trước khi tải
  if (!selectedAngle.preview) {
    captureCurrentAngle()
  }
  
  // Tạo một element a để tải xuống
  const link = document.createElement('a')
  link.href = selectedAngle.preview
  link.download = `adidas-custom-shoe-${selectedAngle.name}.png`
  document.body.appendChild(link)
  link.click()
  document.body.removeChild(link)
  
  console.log(`Đã tải ảnh từ góc nhìn: ${selectedAngle.name}`)
}

// Hàm xử lý khi nhấn nút tải ảnh (giữ lại để tương thích ngược)
const handleDownloadImage = () => {
  if (!renderer) return
  
  // Render scene một lần nữa để đảm bảo hình ảnh mới nhất
  renderer.render(scene, camera)
  
  // Lấy dữ liệu hình ảnh từ canvas
  const imageData = renderer.domElement.toDataURL('image/png')
  
  // Tạo một element a để tải xuống
  const link = document.createElement('a')
  link.href = imageData
  link.download = 'adidas-custom-shoe.png'
  document.body.appendChild(link)
  link.click()
  document.body.removeChild(link)
  
  console.log('Đã tải ảnh mô hình 3D')
}

// Hàm toggle kích thước canvas
const toggleCanvasSize = () => {
  isCanvasExpanded.value = !isCanvasExpanded.value
  setTimeout(() => {
    if (renderer && camera) {
      onWindowResize()
    }
  }, 300)
}

// Three.js objects
let scene, camera, renderer, controls, model
const materials = {}
const customTextures = {}

// State cho văn bản tùy chỉnh
const customText = ref('')

// State cho tab đang active
const activeTab = ref('color')

// State cho màu văn bản đã chọn
const selectedTextColor = ref('#ffffff') // Mặc định là màu trắng

// State cho các tham số texture
const textureParams = reactive({
  scale: 1.0,
  repeatX: 1.1,
  repeatY: 1.6,
  offsetX: 0,
  offsetY: 0,
  rotation: 0.91,
  brightness: 2.0
})

// State cho upload ảnh
const imageInput = ref(null)
const selectedImage = ref(null)
const selectedImageName = ref('')
const previewImageUrl = ref('')

// Hàm xử lý khi người dùng chọn ảnh
const onImageSelected = (event) => {
  const file = event.target.files[0]
  if (file) {
    selectedImage.value = file
    selectedImageName.value = file.name.length > 20 
      ? file.name.substring(0, 17) + '...' 
      : file.name
    
    // Tạo URL cho hình ảnh xem trước
    previewImageUrl.value = URL.createObjectURL(file)
  }
}

// Hàm áp dụng ảnh đã upload làm texture cho mesh
const applyImageToMesh = () => {
  if (!selectedImage.value) {
    alert('Vui lòng chọn ảnh trước khi áp dụng')
    return
  }
  
  const selectedPart = components[selectedComponentIndex.value].value
  if (!selectedPart || !materials[selectedPart]) {
    alert('Không tìm thấy phần đã chọn')
    return
  }
  
  const material = materials[selectedPart]
  
  // Lưu lại màu hiện tại của mesh
  const currentColor = material.color.clone()
  
  // Tạo đường dẫn URL cho ảnh đã chọn
  const imageUrl = URL.createObjectURL(selectedImage.value)
  
  // Tạo texture từ ảnh đã upload
  const textureLoader = new THREE.TextureLoader()
  textureLoader.load(
    imageUrl,
    (texture) => {
      // Cấu hình texture
      texture.anisotropy = renderer ? renderer.capabilities.getMaxAnisotropy() : 1
      texture.wrapS = THREE.RepeatWrapping
      texture.wrapT = THREE.RepeatWrapping
      
      // Đặt các thông số texture từ textureParams
      texture.repeat.set(textureParams.repeatX, textureParams.repeatY)
      texture.offset.set(textureParams.offsetX, textureParams.offsetY)
      texture.rotation = textureParams.rotation
      
      // Lưu texture cũ nếu chưa lưu
      if (!customTextures[selectedPart]) {
        customTextures[selectedPart] = {
          originalMap: material.map,
          originalColor: material.color.clone(),
          texture: texture
        }
      } else {
        customTextures[selectedPart].texture = texture
      }
      
      // Áp dụng texture mới với blending
      material.map = texture
      
      // Thiết lập blending mode để có thể kết hợp texture với màu
      material.blending = THREE.NormalBlending
      
      // Đặt màu phù hợp với độ sáng
      const brightness = textureParams.brightness
      material.color.set(new THREE.Color(brightness, brightness, brightness))
      
      // Thiết lập các thuộc tính material để hiển thị đúng
      material.transparent = true
      material.needsUpdate = true
      
      console.log(`Đã áp dụng ảnh "${selectedImageName.value}" lên ${selectedPart}`)
      
      // Giải phóng URL để tránh rò rỉ bộ nhớ
      URL.revokeObjectURL(imageUrl)
    },
    undefined,
    (error) => {
      console.error('Lỗi khi tải ảnh:', error)
      alert('Đã xảy ra lỗi khi tải ảnh, vui lòng thử lại')
      URL.revokeObjectURL(imageUrl)
    }
  )
}

// Hàm cập nhật các tham số texture
const updateTextureParameters = () => {
  const selectedPart = components[selectedComponentIndex.value].value
  if (selectedPart && customTextures[selectedPart] && customTextures[selectedPart].texture) {
    const texture = customTextures[selectedPart].texture
    const material = materials[selectedPart]
    
    // Cập nhật các tham số texture
    texture.repeat.set(textureParams.repeatX, textureParams.repeatY)
    texture.offset.set(textureParams.offsetX, textureParams.offsetY)
    texture.rotation = textureParams.rotation
    
    // Cập nhật độ sáng
    const brightness = textureParams.brightness
    material.color.set(new THREE.Color(brightness, brightness, brightness))
    
    // Cập nhật material
    material.needsUpdate = true
  }
}

// Watch các thay đổi trong textureParams để cập nhật texture
watch(textureParams, () => {
  updateTextureParameters()
}, { deep: true })

// Hàm áp dụng văn bản lên mesh đã chọn
const applyTextToMesh = () => {
  if (!customText.value.trim()) {
    alert('Vui lòng nhập văn bản trước khi áp dụng')
    return
  }
  
  const selectedPart = components[selectedComponentIndex.value].value
  if (!selectedPart || !materials[selectedPart]) {
    alert('Không tìm thấy phần đã chọn')
    return
  }
  
  const material = materials[selectedPart]
  
  // Lưu lại màu hiện tại của mesh
  const currentColor = material.color.clone()
  
  // Tạo texture từ canvas chứa text
  const canvas = document.createElement('canvas')
  const context = canvas.getContext('2d')
  
  // Kích thước canvas
  canvas.width = 1024
  canvas.height = 1024
  
  // Vẽ nền với màu tương tự như màu hiện tại của mesh
  context.fillStyle = `rgb(${Math.floor(currentColor.r * 255)}, ${Math.floor(currentColor.g * 255)}, ${Math.floor(currentColor.b * 255)})`
  context.fillRect(0, 0, canvas.width, canvas.height)
  
  // Tính kích thước văn bản phù hợp với độ dài
  const textLength = customText.value.length
  const fontSize = Math.min(150, 600 / Math.max(1, textLength / 3))
  
  // Lưu trạng thái canvas hiện tại
  context.save()
  
  // Xoay canvas 180 độ và lật ngang để văn bản hiển thị đúng
  context.translate(canvas.width / 2, canvas.height / 2)
  context.rotate(Math.PI) // Xoay 180 độ (π radian)
  context.scale(-1, 1) // Lật ngược chiều ngang
  context.translate(-canvas.width / 2, -canvas.height / 2)
  
  // Vẽ text với màu đã chọn nhưng với viền đen để dễ nhìn trên mọi nền
  context.font = `bold ${fontSize}px Arial, sans-serif`
  
  // Vẽ viền
  context.strokeStyle = 'black'
  context.lineWidth = fontSize / 8 // Tăng độ đậm của viền
  context.textAlign = 'center'
  context.textBaseline = 'middle'
  context.strokeText(customText.value, canvas.width / 2, canvas.height / 2)
  
  // Vẽ text với màu trắng cố định
  context.fillStyle = '#ffffff' // Màu trắng cố định
  // Giảm hiệu ứng shadow hoặc loại bỏ nó để text rõ nét hơn
  context.shadowColor = 'rgba(0, 0, 0, 0.7)' // Tăng độ đậm của shadow
  context.shadowBlur = 3 // Giảm độ mờ
  context.shadowOffsetX = 1 // Giảm độ lệch
  context.shadowOffsetY = 1 // Giảm độ lệch
  context.fillText(customText.value, canvas.width / 2, canvas.height / 2)
  
  // Vẽ lại văn bản một lần nữa không có shadow để tăng độ rõ nét
  context.shadowColor = 'transparent'
  context.shadowBlur = 0
  context.shadowOffsetX = 0
  context.shadowOffsetY = 0
  context.fillStyle = '#ffffff' // Màu trắng cố định
  context.fillText(customText.value, canvas.width / 2, canvas.height / 2)
  
  // Khôi phục trạng thái canvas ban đầu
  context.restore()
  
  // Tạo texture từ canvas
  const texture = new THREE.CanvasTexture(canvas)
  texture.anisotropy = renderer ? renderer.capabilities.getMaxAnisotropy() : 1
  
  // Thiết lập các tham số cho texture
  texture.wrapS = THREE.RepeatWrapping
  texture.wrapT = THREE.RepeatWrapping
  texture.repeat.set(1, 1)
  
  // Lưu texture cũ nếu chưa lưu
  if (!customTextures[selectedPart]) {
    customTextures[selectedPart] = {
      originalMap: materials[selectedPart].map,
      originalColor: materials[selectedPart].color.clone(),
      texture: texture
    }
  } else {
    customTextures[selectedPart].texture = texture
  }
  
  // Áp dụng texture mới
  material.map = texture
  
  // Sử dụng blending mode bình thường thay vì custom
  material.blending = THREE.NormalBlending
  
  // Giữ nguyên màu hiện tại của material
  material.color.copy(currentColor)
  
  // Thiết lập các thuộc tính material để hiển thị đúng
  material.needsUpdate = true
  
  console.log(`Đã áp dụng văn bản "${customText.value}" vào ${selectedPart}`)
}

// Thêm hàm để xóa texture và phục hồi màu ban đầu
const removeTextFromMesh = () => {
  const selectedPart = components[selectedComponentIndex.value].value
  if (selectedPart && customTextures[selectedPart]) {
    const material = materials[selectedPart]
    
    // Khôi phục texture gốc
    material.map = customTextures[selectedPart].originalMap
    
    // Khôi phục màu gốc nếu người dùng muốn
    if (customTextures[selectedPart].originalColor) {
      material.color.copy(customTextures[selectedPart].originalColor)
    }
    
    // Phục hồi blending mode
    material.blending = THREE.NormalBlending
    
    // Cập nhật material
    material.transparent = material.map ? true : false
    material.needsUpdate = true
    
    // Reset input file và preview image
    if (imageInput.value) {
      imageInput.value.value = ''
    }
    
    // Xóa preview image khi đang ở tab image
    if (activeTab.value === 'image') {
      if (previewImageUrl.value) {
        URL.revokeObjectURL(previewImageUrl.value)
        previewImageUrl.value = ''
      }
      selectedImage.value = null
      selectedImageName.value = ''
    }
    
    console.log(`Đã xóa texture khỏi ${selectedPart}`)
  }
}

// Hàm xử lý khi nhấn nút hoàn thành
const handleDone = () => {
  console.log('Mở modal hoàn thành')
  // Lưu vị trí camera hiện tại
  originalCameraPosition = {
    position: camera ? new THREE.Vector3().copy(camera.position) : null,
    rotation: controls ? controls.target.clone() : null
  }
  
  // Chụp tất cả các góc
  captureAllAngles().then(() => {
    console.log('Đã chụp tất cả các góc, hiển thị modal hoàn thành')
    // Hiển thị modal hoàn thành
    showCompleteModal.value = true
  })
}

// Thêm hàm thêm vào giỏ hàng
const addToCart = () => {
  showCompleteModal.value = false
  
  // Xử lý thêm vào giỏ hàng ở đây
  // Ví dụ: gửi dữ liệu thiết kế lên server
  
  // Thông báo đã thêm vào giỏ hàng
  alert('Sản phẩm đã được thêm vào giỏ hàng thành công!')
  
  // Chuyển hướng về trang customPage sau khi đóng alert
  window.location.href = '/customPage'
}

// Thêm hàm lưu nháp
const saveAsDraft = () => {
  showCompleteModal.value = false
  
  // Thu thập dữ liệu thiết kế
  const designData = {
    productName: 'Adidas Running Shoes (Tùy chỉnh)',
    price: '2.500.000 ₫',
    components: {},
    textureParams: {...textureParams},
    customText: customText.value,
    cameraPosition: camera ? {
      x: camera.position.x,
      y: camera.position.y,
      z: camera.position.z
    } : null,
    timestamp: new Date().toISOString()
  }
  
  // Lưu thông tin về các thay đổi trên từng component
  for (const comp of components) {
    const partName = comp.value
    if (materials[partName]) {
      const material = materials[partName]
      designData.components[partName] = {
        name: comp.name,
        color: '#' + material.color.getHexString(),
        hasTexture: !!material.map
      }
      
      // Kiểm tra và lưu thông tin về texture
      if (customTextures[partName]) {
        designData.components[partName].textureInfo = {
          type: customTextures[partName].texture instanceof THREE.CanvasTexture ? 'text' : 'image',
          // Không thể lưu trực tiếp texture vì quá lớn, chỉ lưu thông tin
          textContent: customText.value
        }
      }
    }
  }
  
  // Chuyển đổi thành chuỗi JSON
  const jsonString = JSON.stringify(designData, null, 2)
  
  // Tạo và tải xuống file
  const blob = new Blob([jsonString], { type: 'application/json' })
  const url = URL.createObjectURL(blob)
  const link = document.createElement('a')
  link.href = url
  link.download = 'adidas-custom-design.json'
  document.body.appendChild(link)
  link.click()
  document.body.removeChild(link)
  URL.revokeObjectURL(url)
  
  // Thông báo đã lưu bản nháp
  alert('Thiết kế đã được lưu vào bản nháp và tải xuống thành công!')
}

// Khởi tạo Three.js
const initThree = () => {
  // Log kích thước container để debug
  console.log('Container dimensions:', container.value.clientWidth, container.value.clientHeight)
  
  // Tạo scene
  scene = new THREE.Scene()
  scene.background = new THREE.Color(0xf0f0f0)

  // Tạo camera
  camera = new THREE.PerspectiveCamera(
    75, 
    container.value.clientWidth / container.value.clientHeight, 
    0.1, 
    1000
  )
  // Đặt vị trí camera để nhìn thấy mô hình tốt nhất
  camera.position.set(0, 1, 8)

  // Tạo renderer với chất lượng cao
  renderer = new THREE.WebGLRenderer({ 
    antialias: true, 
    alpha: true,
    preserveDrawingBuffer: true 
  })
  renderer.setPixelRatio(window.devicePixelRatio)
  renderer.setSize(container.value.clientWidth, container.value.clientHeight)
  renderer.shadowMap.enabled = true
  renderer.shadowMap.type = THREE.PCFSoftShadowMap
  container.value.appendChild(renderer.domElement)

  // Thêm đèn
  // Ánh sáng môi trường
  const ambientLight = new THREE.AmbientLight(0xffffff, 1.0)
  scene.add(ambientLight)

  // Ánh sáng chính (để tạo bóng)
  const mainLight = new THREE.DirectionalLight(0xffffff, 1.5)
  mainLight.position.set(5, 10, 7)
  mainLight.castShadow = true
  mainLight.shadow.mapSize.width = 1024
  mainLight.shadow.mapSize.height = 1024
  scene.add(mainLight)
  
  // Ánh sáng điền (để chiếu sáng các góc tối)
  const fillLight = new THREE.DirectionalLight(0xffffff, 0.8)
  fillLight.position.set(-5, 0, -5)
  scene.add(fillLight)

  // Thêm controls
  controls = new OrbitControls(camera, renderer.domElement)
  controls.enableDamping = true
  controls.dampingFactor = 0.05
  controls.rotateSpeed = 0.7
  controls.minDistance = 3
  controls.maxDistance = 20

  // Tải model
  loadModel()

  // Animation loop
  animate()

  // Handle resize
  window.addEventListener('resize', onWindowResize)
}

// Tải mô hình 3D
const loadModel = () => {
  console.log('Bắt đầu tải mô hình GLB...')
  
  // Tạo DRACOLoader
  const dracoLoader = new DRACOLoader()
  dracoLoader.setDecoderPath('https://www.gstatic.com/draco/versioned/decoders/1.5.5/') // Sử dụng CDN của Google
  console.log('Đã thiết lập DRACOLoader với decoder từ CDN')
  
  // Tạo GLTFLoader và gán DRACOLoader vào
  const loader = new GLTFLoader()
  loader.setDRACOLoader(dracoLoader)
  
  try {
    // Đường dẫn tương đối và tuyệt đối
    const relativeModelPath = '/Adidasrunningshoes.glb';
    const baseUrl = window.location.origin;
    const absoluteModelPath = `${baseUrl}/Adidasrunningshoes.glb`;
    
    console.log('Đường dẫn tương đối:', relativeModelPath);
    console.log('Đường dẫn tuyệt đối:', absoluteModelPath);
    
    // Kiểm tra xem file có tồn tại không
    fetch(relativeModelPath)
      .then(response => {
        if (!response.ok) {
          console.error(`File không tồn tại tại đường dẫn ${relativeModelPath}, mã lỗi: ${response.status}`);
          throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        console.log('Fetch: File tồn tại, kích thước:', response.headers.get('content-length'), 'bytes');
        console.log('Fetch: Loại nội dung:', response.headers.get('content-type'));
        
        // Tải mô hình
        console.log('Bắt đầu tải mô hình...');
        loader.load(
          relativeModelPath,
          (gltf) => onModelLoaded(gltf),
          onLoadProgress,
          (error) => {
            console.error('Lỗi khi tải với đường dẫn tương đối:', error);
            console.error('Chi tiết lỗi:', error.message);
            
            // Thử tải với đường dẫn tuyệt đối
            console.log('Thử tải với đường dẫn tuyệt đối...');
            loader.load(
              absoluteModelPath,
              (gltf) => onModelLoaded(gltf),
              onLoadProgress,
              (absoluteError) => {
                console.error('Lỗi khi tải với đường dẫn tuyệt đối:', absoluteError);
                console.error('Chi tiết lỗi:', absoluteError.message);
              }
            );
          }
        );
      })
      .catch(error => {
        console.error('Lỗi khi kiểm tra tồn tại file:', error);
        
        // Thử tải trực tiếp nếu fetch không thành công
        console.log('Thử tải trực tiếp mà không kiểm tra tồn tại...');
        loader.load(
          absoluteModelPath,
          (gltf) => onModelLoaded(gltf),
          onLoadProgress,
          (directError) => {
            console.error('Lỗi khi tải trực tiếp:', directError);
            console.error('Chi tiết lỗi:', directError.message);
          }
        );
      });
  } catch (e) {
    console.error('Ngoại lệ khi tải mô hình:', e);
    console.error('Stack trace:', e.stack);
  }
}

// Hàm xử lý khi mô hình tải thành công
const onModelLoaded = (gltf) => {
  console.log('Mô hình đã tải thành công!', gltf);
  
  // Xóa mô hình hiện tại nếu có
  if (model) {
    scene.remove(model);
  }
  
  model = gltf.scene;
  
  // Thay đổi kích thước và vị trí
  model.scale.set(50, 50, 50);
  model.position.set(0, -2, 0);
  model.rotation.y = Math.PI / 4;
  
  // Lưu trữ tất cả materials và đảm bảo chúng hiển thị đúng
  console.log('Phân tích các mesh trong mô hình:');
  let meshCount = 0;
  model.traverse((node) => {
    if (node.isMesh) {
      meshCount++;
      console.log(`- Mesh ${meshCount}: ${node.name}`);
      
      // Đảm bảo tất cả vật liệu đều hiển thị đúng
      materials[node.name] = node.material;
      node.castShadow = true;
      node.receiveShadow = true;
      
      // Kiểm tra và cập nhật material
      if (!node.material) {
        console.log(`  Mesh ${node.name} không có material, tạo material mặc định`);
        node.material = new THREE.MeshStandardMaterial({ color: 0x808080 });
      } else {
        console.log(`  Mesh ${node.name} có material:`, node.material.type);
        
        // Điều chỉnh material để hiển thị tốt hơn
        if (node.material.type === 'MeshStandardMaterial') {
          node.material.metalness = 0.3;
          node.material.roughness = 0.4;
        }
      }
    }
  });
  
  if (meshCount === 0) {
    console.warn('CẢNH BÁO: Mô hình không chứa mesh nào!');
    return;
  }
  
  // Thêm vào scene
  scene.add(model);
  console.log('Đã thêm mô hình vào scene thành công');
}

// Hàm theo dõi tiến độ tải
const onLoadProgress = (xhr) => {
  const progress = Math.floor((xhr.loaded / xhr.total) * 100);
  console.log(`Tiến độ tải: ${progress}% (${xhr.loaded} / ${xhr.total} bytes)`);
}

// Animation loop
const animate = () => {
  requestAnimationFrame(animate)
  if (controls) controls.update()
  if (renderer && scene && camera) {
    renderer.render(scene, camera)
  }
}

// Xử lý thay đổi kích thước cửa sổ
const onWindowResize = () => {
  if (camera && renderer && container.value) {
    camera.aspect = container.value.clientWidth / container.value.clientHeight
    camera.updateProjectionMatrix()
    renderer.setSize(container.value.clientWidth, container.value.clientHeight)
  }
}

// Danh sách các thành phần của UI
const components = reactive([
  { name: 'Base', value: 'Base' },
  { name: 'Heel', value: 'Heel' },
  { name: 'Lace', value: 'Lace' },
  { name: 'Outsole', value: 'OutSode' },
  { name: 'Midsole', value: 'MidSode001' },
  { name: 'Tip', value: 'Tip' },
  { name: 'Accent', value: 'Accent' }, // Grouped: Accent_inside, Accent_outside, Line_inside, Line_outside
  { name: 'Logo', value: 'Logo' }, // Grouped: Logo_inside, Logo_outside
  { name: 'Details', value: 'Details' }, // Grouped: Cylinder, Cylinder001, Plane012, Plane012_1, Plane005, Plane005_1
])

// State để theo dõi chỉ số thành phần đã chọn
const selectedComponentIndex = ref(0)

// Định nghĩa các mẫu màu với nhãn của chúng
const colors = reactive([
  { name: 'Black', value: '#000000' },
  { name: 'White', value: '#ffffff' },
  { name: 'Off-White', value: '#f5f5f5' },
  { name: 'Gray', value: '#808080' },
  { name: 'Brown', value: '#4a3728' },
  { name: 'Navy Blue', value: '#1c2526' },
  { name: 'Light Blue', value: '#a3c1d4' },
  { name: 'Orange', value: '#ff4500' },
  { name: 'Pink', value: '#ff69b4' },
  { name: 'University Red', value: '#c8102e' },
])

// State để theo dõi màu đã chọn cho phản hồi UI
const selectedColor = ref(colors[0].value)
// Biến để lưu trữ màu tùy chỉnh
const customColorValue = ref('#ff0000')
// Đánh dấu khi màu tùy chỉnh được áp dụng
const customColorApplied = ref(false)

// Cập nhật màu của phần đã chọn
const handleColorClick = (colorValue) => {
  selectedColor.value = colorValue
  customColorApplied.value = false
  updatePartColor(colorValue)
  checkTextContrast()
}

// Xử lý khi người dùng thay đổi màu trong color picker
const handleCustomColorChange = (event) => {
  // Chỉ cập nhật giá trị màu, không áp dụng ngay
  customColorValue.value = event.target.value
}

// Áp dụng màu tùy chỉnh khi người dùng bấm nút Áp dụng
const applyCustomColor = () => {
  selectedColor.value = customColorValue.value
  customColorApplied.value = true
  updatePartColor(customColorValue.value)
  checkTextContrast()
}

// Hàm cập nhật màu sắc
const updatePartColor = (color) => {
  const selectedPart = components[selectedComponentIndex.value].value
  if (selectedPart && materials[selectedPart]) {
    materials[selectedPart].color.set(color)
    
    // Đảm bảo văn bản vẫn hiển thị đúng sau khi thay đổi màu
    if (materials[selectedPart].map && materials[selectedPart].transparent) {
      materials[selectedPart].needsUpdate = true
    }
  }
}

// Xử lý điều hướng của mũi tên
const handlePrevComponent = () => {
  selectedComponentIndex.value = selectedComponentIndex.value === 0 
    ? components.length - 1 
    : selectedComponentIndex.value - 1
}

const handleNextComponent = () => {
  selectedComponentIndex.value = selectedComponentIndex.value === components.length - 1 
    ? 0 
    : selectedComponentIndex.value + 1
}

// Xử lý khi chọn một thành phần từ danh sách
const handleComponentClick = (index) => {
  selectedComponentIndex.value = index
}

// Khởi tạo Three.js sau khi component được mount
onMounted(() => {
  initThree()
})

// Dọn dẹp khi component bị unmount
onBeforeUnmount(() => {
  window.removeEventListener('resize', onWindowResize)
  
  // Dừng animation loop
  if (typeof cancelAnimationFrame === 'function') {
    cancelAnimationFrame(animate)
  }
  
  // Xóa model từ scene và dọn dẹp tài nguyên
  if (model) {
    scene.remove(model)
    model.traverse((node) => {
      if (node.isMesh) {
        if (node.geometry) node.geometry.dispose()
        if (node.material) {
          if (Array.isArray(node.material)) {
            node.material.forEach(material => material.dispose())
          } else {
            node.material.dispose()
          }
        }
      }
    })
  }
  
  // Xóa renderer
  if (renderer) {
    renderer.dispose()
    if (container.value) {
      container.value.removeChild(renderer.domElement)
    }
  }
  
  // Dọn dẹp controls
  if (controls) {
    controls.dispose()
  }
  
  // Dọn dẹp scene
  if (scene) {
    scene.clear()
  }

  // Cleanup URL khi component unmount
  if (previewImageUrl.value) {
    URL.revokeObjectURL(previewImageUrl.value)
  }
})

// Xử lý khi component trong dropdown thay đổi
const handleComponentChange = () => {
  // Không cần làm gì vì v-model đã tự động cập nhật selectedComponentIndex
  console.log('Đã chọn component:', components[selectedComponentIndex.value].name)
}

// Hàm kiểm tra tương phản màu giữa chữ và nền
const checkTextContrast = () => {
  const selectedPart = components[selectedComponentIndex.value].value
  if (selectedPart && materials[selectedPart]) {
    const backgroundColorHex = selectedColor.value;
    const textColorHex = selectedTextColor.value;
    
    // Chuyển màu từ hex sang rgb
    const getRGBFromHex = (hex) => {
      const r = parseInt(hex.substring(1, 3), 16);
      const g = parseInt(hex.substring(3, 5), 16);
      const b = parseInt(hex.substring(5, 7), 16);
      return [r, g, b];
    }
    
    const bgRGB = getRGBFromHex(backgroundColorHex);
    const textRGB = getRGBFromHex(textColorHex);
    
    // Tính khoảng cách Euclidean giữa 2 màu
    const distance = Math.sqrt(
      Math.pow(bgRGB[0] - textRGB[0], 2) +
      Math.pow(bgRGB[1] - textRGB[1], 2) +
      Math.pow(bgRGB[2] - textRGB[2], 2)
    );
    
  }
}
</script>

<style>
.custom-detail-page {
  height: 100vh;
  display: flex;
  flex-direction: column;
  align-items: center;
  position: relative;
  background-color: #f9f9f9;
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
  height: 60vh;
}

.canvas-resizer {
  position: absolute;
  left: 10px;
  top: 50%;
  transform: translateY(-50%);
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
  flex-wrap: wrap;
  justify-content: center;
  gap: 20px;
  width: 100%;
  max-width: 1200px;
}

/* Card styles */
.components-card {
  background-color: white;
  border-radius: 10px;
  padding: 20px;
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
  background: none;
  border: none;
  padding: 10px 20px;
  font-size: 14px;
  font-weight: 500;
  color: #666;
  cursor: pointer;
  border-bottom: 2px solid transparent;
  transition: all 0.2s ease;
}

.tab-button:hover {
  color: #333;
}

.tab-button.active {
  color: #000;
  border-bottom: 2px solid #000;
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
  gap: 10px;
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
</style>