<template>
  <div class="custom-detail-page">
    <!-- Thông tin sản phẩm ở góc trái trên -->
    <div class="product-info">
      <h2 class="product-name">Adidas Running Shoes</h2>
      <p class="product-price">2.500.000 ₫</p>
      <p class="product-surcharge" v-if="surcharge > 0">Phụ phí: {{ formatPrice(surcharge) }}</p>
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
              <p><strong>Tên sản phẩm:</strong> Adidas Running Shoes</p>
              <p><strong>Giá gốc:</strong> 2.500.000 ₫</p>
              <p v-if="surcharge > 0"><strong>Phụ phí tùy chỉnh:</strong> {{ formatPrice(surcharge) }}</p>
              <p><strong>Tổng tiền:</strong> {{ formatPrice(2500000 + surcharge) }}</p>
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
import { ref, onMounted, onBeforeUnmount, reactive, computed, watch } from 'vue'
import * as THREE from 'three'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls'
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader'
import { DRACOLoader } from 'three/examples/jsm/loaders/DRACOLoader'
import { useRoute } from 'vue-router'

// Container reference
const container = ref(null)

// Phụ phí tùy chỉnh
const surcharge = ref(0)

// State for canvas size
const isCanvasExpanded = ref(false)

// State for modals
const showCaptureModal = ref(false)
const showCompleteModal = ref(false)
const selectedAngleIndex = ref(0)
const captureAngles = reactive([
  { name: 'Mặt sau', preview: null, position: { x: -1.5, y: 0.5, z: -1.5 } },
  { name: 'Bên trái', preview: null, position: { x: -1.5, y: 0.5, z: 1.5 } },
  { name: 'Bên phải', preview: null, position: { x: 1.5, y: 0.5, z: -1.5 } },
  { name: 'Góc nhìn trên', preview: null, position: { x: 1.5, y: 6, z: 1.5 } },
  { name: 'Góc nhìn dưới', preview: null, position: { x: -1.5, y: -6, z: 1.5 } }
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

// Part colors and textures (fixed typos and aligned with components)
const partColors = reactive({
  Accent_inside: '#ffffff',
  Accent_outside: '#ffffff',
  Base: '#ffffff',
  Cover: '#ffffff',
  Cylinder: '#ffffff',
  Cylinder001: '#ffffff',
  Heel: '#ffffff',
  Lace: '#ffffff',
  Line_inside: '#ffffff',
  Line_outside: '#ffffff',
  Logo_inside: '#ffffff',
  Logo_outside: '#ffffff',
  MidSode: '#ffffff', 
  MidSode001: '#ffffff', 
  OutSode: '#ffffff', 
  Tip: '#ffffff',
  Plane012: '#ffffff',
  Plane012_1: '#ffffff',
  Plane005: '#ffffff',
  Tongue: '#ffffff'
})

const partTextures = reactive({
  Accent_inside: null,
  Accent_outside: null,
  Base: null,
  Cover: null,
  Cylinder: null,
  Cylinder001: null,
  Heel: null,
  Lace: null,
  Line_inside: null,
  Line_outside: null,
  Logo_inside: null,
  Logo_outside: null,
  MidSode: null, 
  MidSode001: null, 
  OutSode: null, 
  Tip: null,
  Plane012: null,
  Plane012_1: null,
  Plane005: null,
  Tongue: null
})

// Components list
const components = reactive([
  { name: 'Base', value: 'Base' },
  { name: 'Heel', value: 'Heel' },
  { name: 'Lace', value: 'Lace' },
  { name: 'Outsole', value: 'OutSode' },
  { name: 'MidSole', value: 'MidSole' }, 
  { name: 'Tip', value: 'Tip' },
  { name: 'Accent', value: 'Accent' },
  { name: 'Logo', value: 'Logo' },
  { name: 'Details', value: 'Details' }
])

// Part groups 
const partGroups = reactive({
  Accent: ['Accent_inside', 'Accent_outside', 'Line_inside', 'Line_outside'],
  Logo: ['Logo_inside', 'Logo_outside'],
  MidSole: ['MidSode', 'Plane012', 'Plane012_1'], 
  Details: ['Cylinder', 'Cylinder001', 'Plane005']
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
    controls.target.set(0, 0.5, 0)
    const duration = 500
    const startTime = Date.now()

    function animateCamera() {
      const elapsed = Date.now() - startTime
      const progress = Math.min(elapsed / duration, 1)
      const easeProgress = progress < 0.5 ? 2 * progress * progress : 1 - Math.pow(-2 * progress + 2, 2) / 2
      camera.position.lerpVectors(startPosition, targetPosition, easeProgress)
      camera.lookAt(0, 0.5, 0)
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
    camera.lookAt(0, 0.5, 0)
    renderer.render(scene, camera)
    captureAngles[selectedAngleIndex.value].preview = renderer.domElement.toDataURL('image/png')
  }
}

const captureAllAngles = async () => {
  const initialPosition = camera.position.clone()
  const initialTarget = controls.target.clone()
  controls.target.set(0, 0.5, 0)

  for (let i = 0; i < captureAngles.length; i++) {
    selectedAngleIndex.value = i
    const anglePosition = captureAngles[i].position
    camera.position.set(anglePosition.x, anglePosition.y, anglePosition.z)
    camera.lookAt(0, 0.5, 0)
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
  // Tính phụ phí trước khi hiển thị modal
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
    
    // Chuyển đổi ảnh thành base64 để lưu trữ
    const reader = new FileReader()
    reader.onload = (e) => {
      // Lưu dữ liệu base64 của ảnh
      previewImageUrl.value = e.target.result
    }
    reader.readAsDataURL(file)
  }
}

const applyImageToMesh = () => {
  if (!selectedImage.value) {
    alert('Vui lòng chọn ảnh trước khi áp dụng')
    return
  }

  const selectedPart = components[selectedComponentIndex.value].value
  const partsToUpdate = selectedPart in partGroups ? partGroups[selectedPart] : [selectedPart]

  // Sử dụng dữ liệu base64 đã được lưu
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

      // Nếu đang cập nhật Lace, sử dụng phương pháp đặc biệt
      if (partsToUpdate.includes('Lace')) {
        console.log('Đang áp dụng texture cho dây giày')
        
        // Tìm tất cả mesh dây giày
        const laceMeshes = findAllLaceMeshes()
        
        if (laceMeshes.length > 0) {
          // Xử lý từng mesh dây giày tìm thấy
          laceMeshes.forEach(mesh => {
            console.log(`Đang áp dụng texture cho mesh ${mesh.name}`)
            
            // Tạo material mới với texture
            const newMaterial = new THREE.MeshStandardMaterial({
              map: texture,
              color: new THREE.Color(textureParams.brightness, textureParams.brightness, textureParams.brightness),
              transparent: true,
              side: THREE.DoubleSide  // Hiển thị cả hai mặt của mesh
            })
            
            // Lưu material gốc
            if (!customTextures['Lace']) {
              customTextures['Lace'] = {
                originalMap: mesh.material.map,
                originalColor: mesh.material.color ? mesh.material.color.clone() : new THREE.Color('#ffffff'),
                texture,
                imageData: imageUrl
              }
            }
            
            // Clone mesh với material mới
            try {
              // Thay thế material
              mesh.material = newMaterial
              mesh.material.needsUpdate = true
              
              // Cập nhật tham chiếu trong mảng materials
              materials['Lace'] = newMaterial
              partTextures['Lace'] = texture
              
              console.log(`Đã áp dụng texture cho mesh ${mesh.name}`)
            } catch (error) {
              console.error(`Lỗi khi áp dụng texture cho mesh ${mesh.name}:`, error)
            }
          })
        } else {
          console.warn('Không tìm thấy mesh dây giày nào')
        }
      }

      // Xử lý các phần khác như bình thường
      partsToUpdate.forEach((part) => {
        if (part !== 'Lace' && materials[part]) {
          console.log(`Đang áp dụng texture cho phần: ${part}`)
          
          if (!customTextures[part]) {
            customTextures[part] = {
              originalMap: materials[part].map,
              originalColor: materials[part].color.clone(),
              texture,
              imageData: imageUrl // Lưu trữ dữ liệu ảnh base64
            }
          } else {
            customTextures[part].texture = texture
            customTextures[part].imageData = imageUrl // Cập nhật dữ liệu ảnh base64
          }
          partTextures[part] = texture
          materials[part].map = texture
          materials[part].color.set(new THREE.Color(textureParams.brightness, textureParams.brightness, textureParams.brightness))
          materials[part].transparent = true
          materials[part].needsUpdate = true
        }
      })

      renderer.render(scene, camera)
      
      // Tính toán phụ phí sau khi thêm ảnh
      calculateSurcharge()
    },
    undefined,
    (error) => {
      console.error('Lỗi khi tải ảnh:', error)
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

  // Thiết lập chung cho canvas
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

  // Tạo texture từ canvas
  const texture = new THREE.CanvasTexture(canvas)
  texture.anisotropy = renderer.capabilities.getMaxAnisotropy()
  texture.wrapS = texture.wrapT = THREE.RepeatWrapping
  texture.repeat.set(1, 1)
  texture.needsUpdate = true

  // Nếu đang cập nhật Lace, sử dụng phương pháp đặc biệt
  if (partsToUpdate.includes('Lace')) {
    console.log('Đang áp dụng text texture cho dây giày')
    
    // Tìm tất cả mesh dây giày
    const laceMeshes = findAllLaceMeshes()
    
    if (laceMeshes.length > 0) {
      // Xử lý từng mesh dây giày tìm thấy
      laceMeshes.forEach(mesh => {
        console.log(`Đang áp dụng text texture cho mesh ${mesh.name}`)
        
        // Tạo material mới với texture
        const newMaterial = new THREE.MeshStandardMaterial({
          map: texture,
          transparent: true,
          side: THREE.DoubleSide  // Hiển thị cả hai mặt của mesh
        })
        
        // Lưu material gốc
        if (!customTextures['Lace']) {
          customTextures['Lace'] = {
            originalMap: mesh.material.map,
            originalColor: mesh.material.color ? mesh.material.color.clone() : new THREE.Color('#ffffff'),
            texture
          }
        } else {
          customTextures['Lace'].texture = texture
        }
        
        // Thay thế material
        mesh.material = newMaterial
        mesh.material.needsUpdate = true
        
        // Cập nhật tham chiếu trong mảng materials
        materials['Lace'] = newMaterial
        partTextures['Lace'] = texture
        
        console.log(`Đã áp dụng text texture cho mesh ${mesh.name}`)
      })
    } else {
      console.warn('Không tìm thấy mesh dây giày nào')
    }
  }

  // Xử lý các phần khác như bình thường
  partsToUpdate.forEach((part) => {
    if (part !== 'Lace' && materials[part]) {
      console.log(`Đang áp dụng text texture cho phần: ${part}`)
      
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
    }
  })

  renderer.render(scene, camera)
  
  // Tính toán phụ phí sau khi thêm văn bản
  calculateSurcharge()
}

const removeTextFromMesh = () => {
  const selectedPart = components[selectedComponentIndex.value].value
  const partsToUpdate = selectedPart in partGroups ? partGroups[selectedPart] : [selectedPart]

  // Nếu đang cập nhật Lace, sử dụng phương pháp đặc biệt
  if (partsToUpdate.includes('Lace')) {
    console.log('Đang xóa texture cho dây giày')
    
    // Tìm tất cả mesh dây giày
    const laceMeshes = findAllLaceMeshes()
    
    if (laceMeshes.length > 0) {
      // Xử lý từng mesh dây giày tìm thấy
      laceMeshes.forEach(mesh => {
        console.log(`Đang xóa texture cho mesh ${mesh.name}`)
        
        if (customTextures['Lace']) {
          // Tạo material mới phục hồi trạng thái gốc
          const newMaterial = new THREE.MeshStandardMaterial({
            map: customTextures['Lace'].originalMap,
            side: THREE.DoubleSide
          })
          
          // Khôi phục màu gốc
          if (customTextures['Lace'].originalColor) {
            newMaterial.color.copy(customTextures['Lace'].originalColor)
          }
          
          // Thay thế material
          mesh.material = newMaterial
          mesh.material.needsUpdate = true
          
          console.log(`Đã xóa texture cho mesh ${mesh.name}`)
        }
      })
      
      // Xóa khỏi customTextures
      partTextures['Lace'] = null
      delete customTextures['Lace']
    } else {
      console.warn('Không tìm thấy mesh dây giày nào')
    }
  }
  
  // Xử lý các phần khác như bình thường
  partsToUpdate.forEach((part) => {
    if (part !== 'Lace' && materials[part] && customTextures[part]) {
      console.log(`Đang xóa texture cho phần: ${part}`)
      
      materials[part].map = customTextures[part].originalMap
      materials[part].color.copy(customTextures[part].originalColor)
      materials[part].transparent = materials[part].map ? true : false
      materials[part].needsUpdate = true
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
  
  // Tính toán phụ phí sau khi xóa texture
  calculateSurcharge()
}

const addToCart = () => {
  showCompleteModal.value = false
  
  // Kiểm tra xem đang ở chế độ chỉnh sửa không
  const urlParams = new URLSearchParams(window.location.search)
  const isEditing = urlParams.get('edit') === 'true'
  const editId = urlParams.get('id')
  
  // Tính phụ phí cuối cùng trước khi thêm vào giỏ hàng
  calculateSurcharge()
  
  // Tạo dữ liệu sản phẩm với thiết kế tùy chỉnh
  const productData = {
    id: isEditing && editId ? parseInt(editId) : Date.now(), // Sử dụng ID hiện có nếu đang chỉnh sửa
    name: 'Adidas Running Shoes',
    price: 2500000,
    surcharge: surcharge.value, // Sử dụng giá trị surcharge đã tính
    image: captureAngles[1].preview, // Lưu hình ảnh mặc định (mặt sau)
    designData: {
      colors: {},
      textures: {},
      imagesData: {}, // Thêm object để lưu trữ dữ liệu ảnh base64
      customText: customText.value,
      textureParams: { ...textureParams },
      timestamp: new Date().toISOString()
    },
    previewImages: captureAngles.map(angle => angle.preview) // Lưu tất cả các góc nhìn
  }
  
  // Lưu thông tin màu sắc và texture
  for (const comp of components) {
    const partName = comp.value
    if (materials[partName]) {
      const hexColor = '#' + materials[partName].color.getHexString()
      productData.designData.colors[partName] = hexColor
      
      if (customTextures[partName]) {
        const textureType = customTextures[partName].texture instanceof THREE.CanvasTexture ? 'text' : 'image'
        
        productData.designData.textures[partName] = {
          type: textureType,
          textContent: customText.value
        }
        
        // Lưu trữ dữ liệu ảnh base64 nếu có
        if (textureType === 'image' && customTextures[partName].imageData) {
          productData.designData.imagesData[partName] = customTextures[partName].imageData
        }
      }
    }
  }
  
  // Lấy giỏ hàng hiện tại từ localStorage
  let cart = []
  const savedCart = localStorage.getItem('cart')
  if (savedCart) {
    cart = JSON.parse(savedCart)
  }
  
  // Tính tổng tiền
  const totalPrice = 2500000 + surcharge.value;
  const formattedTotalPrice = formatPrice(totalPrice);
  const formattedSurcharge = surcharge.value > 0 ? `\nPhụ phí tùy chỉnh: ${formatPrice(surcharge.value)}` : '';
  
  if (isEditing && editId) {
    // Cập nhật sản phẩm hiện có thay vì thêm mới
    const itemIndex = cart.findIndex(item => item.id === parseInt(editId))
    if (itemIndex !== -1) {
      cart[itemIndex] = productData
    } else {
      // Thêm mới nếu không tìm thấy (hiếm khi xảy ra)
      cart.push(productData)
    }
    alert(`Đã cập nhật thiết kế của sản phẩm trong giỏ hàng!\nGiá gốc: 2.500.000 ₫${formattedSurcharge}\nTổng tiền: ${formattedTotalPrice}`)
  } else {
    // Thêm sản phẩm mới vào giỏ hàng
    cart.push(productData)
    alert(`Sản phẩm thiết kế đã được thêm vào giỏ hàng thành công!\nGiá gốc: 2.500.000 ₫${formattedSurcharge}\nTổng tiền: ${formattedTotalPrice}`)
  }
  
  // Lưu giỏ hàng vào localStorage
  localStorage.setItem('cart', JSON.stringify(cart))
  
  window.location.href = '/cartcustomPage'
}

const saveAsDraft = () => {
  showCompleteModal.value = false;
  
  // Tính phụ phí cuối cùng
  calculateSurcharge();
  
  // Tạo đối tượng thiết kế với ID mới
  const productData = {
    id: Date.now(),
    name: 'Adidas Running Shoes (Nháp)',
    price: 2500000,
    surcharge: surcharge.value,
    image: captureAngles[1].preview, // Lưu hình ảnh mặc định
    designData: {
      colors: {},
      textures: {},
      imagesData: {},
      customText: customText.value,
      textureParams: { ...textureParams },
      timestamp: new Date().toISOString()
    },
    previewImages: captureAngles.map(angle => angle.preview) // Lưu tất cả các góc nhìn
  };
  
  // Lưu thông tin màu sắc và texture
  for (const comp of components) {
    const partName = comp.value;
    if (materials[partName]) {
      productData.designData.colors[partName] = '#' + materials[partName].color.getHexString();
      
      if (customTextures[partName]) {
        const textureType = customTextures[partName].texture instanceof THREE.CanvasTexture ? 'text' : 'image';
        
        productData.designData.textures[partName] = {
          type: textureType,
          textContent: customText.value
        };
        
        // Lưu trữ dữ liệu ảnh base64 nếu có
        if (textureType === 'image' && customTextures[partName].imageData) {
          productData.designData.imagesData[partName] = customTextures[partName].imageData;
        }
      }
    }
  }
  
  // Lấy danh sách nháp hiện tại từ localStorage
  let drafts = [];
  const savedDrafts = localStorage.getItem('designDrafts');
  if (savedDrafts) {
    drafts = JSON.parse(savedDrafts);
  }
  
  // Thêm thiết kế mới vào danh sách nháp
  drafts.push(productData);
  
  // Lưu danh sách nháp vào localStorage
  localStorage.setItem('designDrafts', JSON.stringify(drafts));
  
  // Tính tổng tiền
  const totalPrice = 2500000 + surcharge.value;
  const formattedTotalPrice = formatPrice(totalPrice);
  const formattedSurcharge = surcharge.value > 0 ? `\nPhụ phí tùy chỉnh: ${formatPrice(surcharge.value)}` : '';
  
  // Hiển thị thông báo với tổng tiền
  alert(`Thiết kế đã được lưu vào bản nháp!\nGiá gốc: 2.500.000 ₫${formattedSurcharge}\nTổng tiền: ${formattedTotalPrice}`);
  window.location.href = '/mycustomPage';
}

// Three.js initialization
const initThree = () => {
  scene = new THREE.Scene()
  scene.background = new THREE.Color(0xf0f0f0)

  camera = new THREE.PerspectiveCamera(75, container.value.clientWidth / container.value.clientHeight, 0.1, 1000)
  camera.position.set(1.5, 0.5, 1.5)

  renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true, preserveDrawingBuffer: true })
  renderer.setPixelRatio(window.devicePixelRatio)
  renderer.setSize(container.value.clientWidth, container.value.clientHeight)
  renderer.shadowMap.enabled = true
  renderer.shadowMap.type = THREE.PCFSoftShadowMap
  container.value.appendChild(renderer.domElement)

  // Lighting setup
  const ambientLight = new THREE.AmbientLight(0xffffff, 0.6)
  scene.add(ambientLight)

  const mainLight = new THREE.DirectionalLight(0xffffff, 1.2)
  mainLight.position.set(5, 10, 5)
  mainLight.castShadow = true
  mainLight.shadow.mapSize.width = 1024
  mainLight.shadow.mapSize.height = 1024
  scene.add(mainLight)

  const fillLight = new THREE.DirectionalLight(0xffffff, 0.5)
  fillLight.position.set(-5, 3, -5)
  scene.add(fillLight)

  const topLight = new THREE.DirectionalLight(0xffffff, 0.4)
  topLight.position.set(0, 10, 0)
  scene.add(topLight)

  controls = new OrbitControls(camera, renderer.domElement)
  controls.enableDamping = true
  controls.dampingFactor = 0.05
  controls.rotateSpeed = 0.7
  controls.minDistance = 1
  controls.maxDistance = 0
  controls.target.set(0, 0.5, 0)

  loadModel()
  animate()
  window.addEventListener('resize', onWindowResize)
}

const loadModel = () => {
  const dracoLoader = new DRACOLoader()
  dracoLoader.setDecoderPath('https://www.gstatic.com/draco/versioned/decoders/1.5.5/')
  const loader = new GLTFLoader()
  loader.setDRACOLoader(dracoLoader)

  const modelPath = '/Adidasrunningshoes.glb'
  loader.load(
    modelPath,
    (gltf) => onModelLoaded(gltf),
    (xhr) => console.log(`Tiến độ tải: ${Math.floor((xhr.loaded / xhr.total) * 100)}%`),
    (error) => console.error('Lỗi khi tải mô hình:', error)
  )
}

const onModelLoaded = (gltf) => {
  if (model) {
    scene.remove(model)
  }

  model = gltf.scene
  model.scale.set(5, 5, 5)
  model.position.set(0, 0.3, 0)
  model.rotation.y = Math.PI / 4

  const foundMeshes = []
  const meshMaterialMap = {}
  
  // Tìm kiếm tất cả mesh có thể là dây giày
  const possibleLaceMeshes = []
  
  model.traverse((node) => {
    if (node.isMesh) {
      foundMeshes.push(node.name)
      node.castShadow = true
      node.receiveShadow = true
      
      // Tìm các mesh có thể là dây giày
      if (node.name.toLowerCase().includes('lace') || 
          node.name.toLowerCase().includes('shoelace') || 
          node.name.toLowerCase().includes('string') ||
          node.name.toLowerCase().includes('cord')) {
        possibleLaceMeshes.push({
          name: node.name,
          uuid: node.uuid,
          material: node.material ? node.material.type : 'không có material',
          materialColor: node.material && node.material.color ? node.material.color.getHexString() : 'không có màu'
        })
      }
      
      if (node.material) {
        meshMaterialMap[node.name] = node.material
        
        // Kiểm tra chi tiết về material của mesh nếu là Lace
        if (node.name === 'Lace') {
          console.log('Chi tiết về mesh Lace:', {
            name: node.name,
            uuid: node.uuid,
            materialType: node.material.type,
            materialProps: Object.keys(node.material),
            hasMap: !!node.material.map,
            materialUUID: node.material.uuid,
            materialTransparent: node.material.transparent,
            materialColor: node.material.color ? node.material.color.getHexString() : 'không có màu'
          });
        }
      } else {
        node.material = new THREE.MeshStandardMaterial({ color: 0x808080 })
        meshMaterialMap[node.name] = node.material
      }
    }
  })

  console.log('Found meshes:', foundMeshes)
  console.log('Mesh material map:', meshMaterialMap)
  console.log('Tìm kiếm mesh Lace:', foundMeshes.filter(name => name.toLowerCase().includes('lace')))
  console.log('Các mesh có thể là dây giày:', possibleLaceMeshes)

  Object.keys(partColors).forEach((partName) => {
    const matchingMesh = foundMeshes.find(meshName => meshName.toLowerCase().includes(partName.toLowerCase()))
    const originalMaterial = matchingMesh ? meshMaterialMap[matchingMesh] : new THREE.MeshStandardMaterial({ color: 0x808080 })

    const texture = partTextures[partName]
    if (texture) {
      texture.repeat.set(textureParams.repeatX * textureParams.scale, textureParams.repeatY * textureParams.scale)
      texture.offset.set(textureParams.offsetX, textureParams.offsetY)
      texture.rotation = textureParams.rotation
      texture.wrapS = texture.wrapT = THREE.RepeatWrapping
      texture.needsUpdate = true
    }

    materials[partName] = new THREE.MeshStandardMaterial({
      color: new THREE.Color(partColors[partName]),
      map: texture,
      normalMap: originalMaterial.normalMap || null,
      roughnessMap: originalMaterial.roughnessMap || null,
      metalness: originalMaterial.metalness || 0.2,
      roughness: originalMaterial.roughness || 0.6,
      envMap: scene.environment,
      envMapIntensity: 1.0
    })

    model.traverse((node) => {
      if (node.isMesh && node.name.toLowerCase().includes(partName.toLowerCase())) {
        node.material = materials[partName]
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
        console.warn(`Phần "${partName}" không tìm thấy trong mô hình!`)
        missingMeshes.push(partName)
      }
    })
  }
  if (missingMeshes.length) {
    console.warn('Các phần không tìm thấy:', missingMeshes.join(', '))
  }
}

const animate = () => {
  requestAnimationFrame(animate)
  controls.update()
  renderer.render(scene, camera)
}

const onWindowResize = () => {
  if (camera && renderer && container.value) {
    camera.aspect = container.value.clientWidth / container.value.clientHeight
    camera.updateProjectionMatrix()
    renderer.setSize(container.value.clientWidth, container.value.clientHeight)
    renderer.render(scene, camera)
  }
}

const handleComponentChange = () => {
  console.log('Đã chọn component:', components[selectedComponentIndex.value].name)
}

// Thêm hàm xử lý thay đổi màu từ color picker
const handleCustomColorChange = () => {
  // Cập nhật màu đã chọn khi người dùng thay đổi màu trong color picker
  selectedColor.value = customColorValue.value
}

// Hàm áp dụng màu đã chọn cho phần đã chọn
const applyCustomColor = () => {
  const selectedPart = components[selectedComponentIndex.value].value
  const partsToUpdate = selectedPart in partGroups ? partGroups[selectedPart] : [selectedPart]
  
  // Nếu đang cập nhật Lace, sử dụng phương pháp đặc biệt
  if (partsToUpdate.includes('Lace')) {
    console.log('Đang áp dụng màu cho dây giày:', customColorValue.value)
    
    // Tìm tất cả mesh dây giày
    const laceMeshes = findAllLaceMeshes()
    
    if (laceMeshes.length > 0) {
      // Xử lý từng mesh dây giày tìm thấy
      laceMeshes.forEach(mesh => {
        console.log(`Đang áp dụng màu cho mesh ${mesh.name}`)
        
        // Lưu material gốc
        if (!customTextures['Lace']) {
          customTextures['Lace'] = {
            originalMap: mesh.material.map,
            originalColor: mesh.material.color ? mesh.material.color.clone() : new THREE.Color('#ffffff')
          }
        }
        
        // Thay thế bằng material mới
        const newMaterial = new THREE.MeshStandardMaterial({
          color: new THREE.Color(customColorValue.value),
          side: THREE.DoubleSide,
          metalness: 0.2,
          roughness: 0.6
        })
        
        // Thay thế material
        mesh.material = newMaterial
        mesh.material.needsUpdate = true
        
        // Cập nhật tham chiếu và màu trong mảng
        materials['Lace'] = newMaterial
        partColors['Lace'] = customColorValue.value
        
        // Xóa texture nếu có
        if (partTextures['Lace']) {
          partTextures['Lace'] = null
        }
        
        console.log(`Đã áp dụng màu cho mesh ${mesh.name}:`, customColorValue.value)
      })
    } else {
      console.warn('Không tìm thấy mesh dây giày nào')
    }
  }
  
  // Xử lý các phần khác như bình thường
  partsToUpdate.forEach((part) => {
    if (part !== 'Lace' && materials[part]) {
      console.log(`Đang áp dụng màu cho phần: ${part}`)
      
      materials[part].color.set(customColorValue.value)
      materials[part].needsUpdate = true
      partColors[part] = customColorValue.value
      
      // Nếu phần này đang có texture, xóa texture
      if (partTextures[part] && customTextures[part]) {
        materials[part].map = null
        materials[part].needsUpdate = true
        partTextures[part] = null
        delete customTextures[part]
      }
    }
  })
  
  // Cập nhật renderer để hiển thị thay đổi
  renderer.render(scene, camera)
  
  // Tính toán phụ phí sau khi đổi màu
  calculateSurcharge()
}

// Thêm hàm để toggle kích thước canvas
const toggleCanvasSize = () => {
  isCanvasExpanded.value = !isCanvasExpanded.value;
  // Cập nhật kích thước renderer sau khi thay đổi
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
      
      // Nếu là texture ảnh, cập nhật độ sáng
      if (customTextures[part] && customTextures[part].imageData) {
        materials[part].color.set(new THREE.Color(textureParams.brightness, textureParams.brightness, textureParams.brightness))
      }
      
      materials[part].needsUpdate = true

      model.traverse((node) => {
        if (node.isMesh && node.name.toLowerCase().includes(part.toLowerCase())) {
          node.material = materials[part]
          node.material.needsUpdate = true
        }
      })
    }
  })
  
  // Render lại scene để hiển thị thay đổi
  renderer.render(scene, camera)
}

// Theo dõi thay đổi của tham số texture để cập nhật thời gian thực
watch(textureParams, updateTextureParameters, { deep: true })

onMounted(() => {
  initThree()
  
  // Kiểm tra xem có đang chỉnh sửa thiết kế không
  const urlParams = new URLSearchParams(window.location.search)
  const isEditing = urlParams.get('edit') === 'true'
  const editId = urlParams.get('id')
  
  if (isEditing && editId) {
    // Tải thiết kế từ localStorage
    const editingDesignJson = localStorage.getItem('editingDesign')
    if (editingDesignJson) {
      try {
        const editingDesign = JSON.parse(editingDesignJson)
        
        if (editingDesign.id.toString() === editId) {
          // Đây là thiết kế đúng, thực hiện tải lại
          console.log('Đang tải lại thiết kế...', editingDesign)
          
          // Khôi phục giá trị phụ phí (nếu có)
          if (editingDesign.surcharge) {
            surcharge.value = editingDesign.surcharge
          }
          
          // Đặt lại customText nếu có
          if (editingDesign.designData && editingDesign.designData.customText) {
            customText.value = editingDesign.designData.customText
          }
          
          // Đặt lại các tham số texture nếu có
          if (editingDesign.designData && editingDesign.designData.textureParams) {
            Object.assign(textureParams, editingDesign.designData.textureParams)
          }
          
          // Sau khi mô hình đã tải xong, sẽ áp dụng các màu sắc và texture
          const applyEditingDesign = () => {
            if (!model) {
              setTimeout(applyEditingDesign, 500) // Thử lại sau 500ms
              return
            }
            
            // Biến để kiểm tra xem sau khi khôi phục có mesh nào được tùy chỉnh không
            let hasCustomizedMeshes = false
            
            // Áp dụng màu sắc
            if (editingDesign.designData && editingDesign.designData.colors) {
              for (const partName in editingDesign.designData.colors) {
                const color = editingDesign.designData.colors[partName]
                if (materials[partName]) {
                  materials[partName].color.set(color)
                  materials[partName].needsUpdate = true
                  partColors[partName] = color
                  
                  // Kiểm tra nếu màu khác mặc định
                  if (color.toLowerCase() !== '#ffffff') {
                    hasCustomizedMeshes = true
                  }
                }
              }
            }
            
            // Khôi phục texture
            if (editingDesign.designData && editingDesign.designData.textures) {
              // Duyệt qua tất cả các phần đã lưu texture
              for (const partName in editingDesign.designData.textures) {
                const textureInfo = editingDesign.designData.textures[partName]
                
                // Kiểm tra xem phần này có tồn tại trong materials không
                if (materials[partName]) {
                  // Phục hồi dựa trên loại texture
                  if (textureInfo.type === 'text' && customText.value) {
                    // Khôi phục texture dạng text - sử dụng hàm applyTextToMesh đã có
                    hasCustomizedMeshes = true
                    applyTextToMesh();
                  } else if (textureInfo.type === 'image' && editingDesign.designData.imagesData && editingDesign.designData.imagesData[partName]) {
                    // Khôi phục texture dạng ảnh từ dữ liệu base64 đã lưu
                    hasCustomizedMeshes = true
                    const imageData = editingDesign.designData.imagesData[partName];
                    const textureLoader = new THREE.TextureLoader();
                    
                    // Đặt URL ảnh để sử dụng lại sau này
                    previewImageUrl.value = imageData;
                    
                    // Tải texture từ dữ liệu ảnh base64
                    textureLoader.load(
                      imageData,
                      (texture) => {
                        texture.anisotropy = renderer.capabilities.getMaxAnisotropy();
                        texture.wrapS = texture.wrapT = THREE.RepeatWrapping;
                        texture.flipY = false;
                        texture.encoding = THREE.sRGBEncoding;
                        texture.repeat.set(textureParams.repeatX * textureParams.scale, textureParams.repeatY * textureParams.scale);
                        texture.offset.set(textureParams.offsetX, textureParams.offsetY);
                        texture.rotation = textureParams.rotation;
                        texture.needsUpdate = true;
                        
                        // Lưu và áp dụng texture
                        customTextures[partName] = {
                          originalMap: materials[partName].map,
                          originalColor: materials[partName].color.clone(),
                          texture,
                          imageData: imageData
                        };
                        partTextures[partName] = texture;
                        materials[partName].map = texture;
                        materials[partName].color.set(new THREE.Color(textureParams.brightness, textureParams.brightness, textureParams.brightness));
                        materials[partName].transparent = true;
                        materials[partName].needsUpdate = true;
                        
                        renderer.render(scene, camera);
                        
                        console.log(`Đã khôi phục texture ảnh cho phần ${partName}`);
                      },
                      undefined,
                      (error) => {
                        console.error(`Lỗi khi tải texture ảnh cho phần ${partName}:`, error);
                      }
                    );
                  }
                }
              }
            }
            
            // Chỉ tính toán lại phụ phí nếu không có mesh nào được tùy chỉnh
            if (!hasCustomizedMeshes) {
              // Đặt lại phụ phí về 0 nếu không có mesh nào được tùy chỉnh
              surcharge.value = 0;
              const productSurchargeElement = document.querySelector('.product-surcharge');
              if (productSurchargeElement) {
                productSurchargeElement.style.display = 'none';
              }
            } else {
              // Cập nhật hiển thị phụ phí
              const productSurchargeElement = document.querySelector('.product-surcharge');
              if (productSurchargeElement && surcharge.value > 0) {
                productSurchargeElement.textContent = `Phụ phí: ${formatPrice(surcharge.value)}`;
                productSurchargeElement.style.display = 'block';
              }
            }
            
            // Xóa thông tin chỉnh sửa sau khi tải xong
            localStorage.removeItem('editingDesign')
          }
          
          applyEditingDesign()
        }
      } catch (e) {
        console.error('Lỗi khi phân tích thiết kế:', e)
      }
    }
  }
})

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

// Thêm hàm định dạng giá tiền
const formatPrice = (price) => {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price || 0);
};

// Hàm tính phụ phí khi thay đổi màu hoặc thêm ảnh
const calculateSurcharge = () => {
  const surchargePerTexture = 50000 // 50.000 VND cho mỗi phần có ảnh
  const surchargePerColor = 30000 // 30.000 VND cho mỗi phần đổi màu
  let totalCustomizedTextures = 0
  let totalCustomizedColors = 0
  
  // Tạo một mảng chứa tất cả các mesh, bao gồm cả mesh con trong các nhóm
  const allMeshes = []
  
  // Thêm các mesh cơ bản
  for (const comp of components) {
    allMeshes.push(comp.value)
  }
  
  // Thêm các mesh con từ các nhóm (partGroups)
  for (const groupName in partGroups) {
    for (const partName of partGroups[groupName]) {
      if (!allMeshes.includes(partName)) {
        allMeshes.push(partName)
      }
    }
  }
  
  // Kiểm tra từng mesh riêng biệt
  for (const partName of allMeshes) {
    if (materials[partName]) {
      // Kiểm tra nếu màu đã thay đổi so với mặc định (#ffffff)
      const hexColor = '#' + materials[partName].color.getHexString()
      if (hexColor.toLowerCase() !== '#ffffff') {
        totalCustomizedColors++
      }
      
      // Kiểm tra các phần đã thêm ảnh
      if (customTextures[partName]) {
        const textureType = customTextures[partName].texture instanceof THREE.CanvasTexture ? 'text' : 'image'
        
        if (textureType === 'image' && customTextures[partName].imageData) {
          totalCustomizedTextures++
        }
      }
    }
  }
  
  // Tính tổng phụ phí
  const calculatedSurcharge = (totalCustomizedTextures * surchargePerTexture) + 
                   (totalCustomizedColors * surchargePerColor)
  
  // Chỉ đặt lại phụ phí về 0 nếu không có phần tùy chỉnh nào
  // Nếu tổng số phần tùy chỉnh là 0, đặt phụ phí về 0
  // Nếu không, cập nhật phụ phí mới
  if (totalCustomizedTextures === 0 && totalCustomizedColors === 0) {
    surcharge.value = 0
  } else {
    surcharge.value = calculatedSurcharge
  }
  
  // Cập nhật phụ phí trên UI
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

// Hàm mới để tìm và xử lý tất cả các mesh dây giày
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
</script>
<style>
.custom-detail-page {
  height: 100vh;
  display: flex;
  flex-direction: column;
  align-items: center;
  position: relative;
  background-color: #f9f9f9;
  overflow-y: auto;
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