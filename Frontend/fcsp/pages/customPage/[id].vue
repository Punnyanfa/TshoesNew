<template>
  <div class="custom-detail-page">
    <!-- Thông tin sản phẩm ở góc trái trên -->
    <div class="product-info">
      <div class="product-name-container">
        <h2 class="product-name">{{ customProductName }}</h2>
        <button class="edit-name-btn" @click="openEditNameModal">
          <i class="fas fa-edit"></i>
        </button>
      </div>
      <p class="product-price">{{ formatPrice(basePrice) }}</p>
      <p class="product-surcharge" v-if="surcharge > 0">Surcharge: {{ formatPrice(surcharge) }}</p>
      
      <!-- Dropdown chọn nhà sản xuất -->
      <div class="manufacturer-selector">
        <label for="manufacturer">Custom Shop:</label>
        <select id="manufacturer" v-model="selectedManufacturer" @change="handleManufacturerChange">
          <option v-for="mfr in manufacturerList" :key="mfr.id" :value="mfr.id">
            {{ mfr.userName }}
          </option>
        </select>
      </div>
    </div>
    
    <!-- Các nút chức năng ở góc phải trên -->
    <div class="action-buttons" style="display: flex; gap: 10px; justify-content: flex-end;">
      <button class="action-button" @click="showSurchargeInfo = true">Surcharge information</button>
      <button class="action-button" @click="openCaptureModal">Download</button>
      <button class="action-button primary-button" @click="handleDone">Complete</button>
    </div>
    
    <!-- Modal hiển thị thông tin phụ phí -->
    <div v-if="showSurchargeInfo" class="surcharge-modal">
      <div class="surcharge-modal-content">
        <div class="surcharge-modal-header">
          <h3>Surcharge Information for Manufacturer #{{ selectedManufacturer }}</h3>
          <button class="close-button" @click="showSurchargeInfo = false">×</button>
        </div>
        <div class="surcharge-modal-body">
          <h4>Custom Surcharge Table</h4>
          <div class="component-surcharge-details">
            <div class="surcharge-detail-table">
              <div class="surcharge-detail-header">
                <div class="detail-col">Component</div>
                <div class="detail-col">Color</div>
                <div class="detail-col">Image</div>
              </div>
              <div v-for="comp in components" :key="comp.value" class="surcharge-detail-row">
                <div class="detail-col">{{ comp.name }}</div>
                <div class="detail-col">
                  {{
                    formatPrice(
                      (apiSurcharges.find(s => s.component === comp.value.toLowerCase() && s.type === 'colorapplication')?.currentAmount) || 0
                    )
                  }}
                </div>
                <div class="detail-col">
                  {{
                    formatPrice(
                      (apiSurcharges.find(s => s.component === comp.value.toLowerCase() && s.type === 'imageapplication')?.currentAmount) || 0
                    )
                  }}
                </div>
              </div>
            </div>
          </div>
          
          <div class="surcharge-note mt-4">
            <p><i>Note: Surcharges are calculated per custom component. Each time you change the color or apply an image to a component, the corresponding surcharge will be added to the product price.</i></p>
          </div>
        </div>
      </div>
    </div>
    
    <!-- Modal chụp ảnh từ nhiều góc -->
    <div v-if="showCaptureModal" class="capture-modal">
      <div class="capture-modal-content">
        <div class="capture-modal-header">
          <h3>Capture 3D Model Images</h3>
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
              <button class="action-button" @click="downloadSelectedAngle">Download Selected Image</button>
            </div>
          </div>
        </div>
      </div>
    </div>
    
    <!-- Modal hoàn thành thiết kế -->
    <div v-if="showCompleteModal" class="capture-modal">
      <div class="capture-modal-content">
        <div class="capture-modal-header">
          <h3>Complete Design</h3>
          <p>Are you sure you want to {{ isEditing ? 'update' : 'save' }} your design?</p>
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
            <h4>Product Details</h4>
            <div class="summary-info">
              <p><strong>Product Name:</strong> {{ customProductName }}</p>
              <p><strong>Base Price:</strong> {{ formatPrice(basePrice) }}</p>
              <p v-if="surcharge > 0"><strong>Surcharge:</strong> {{ formatPrice(surcharge) }}</p>
              <p><strong>Total:</strong> {{ formatPrice(basePrice + surcharge) }}</p>
            </div>
          </div>
          
          <div class="complete-actions">
            <button v-if="!isEditing" class="action-button" @click="saveAsDraft">Save as Draft</button>
            <button v-if="isEditing" class="action-button primary-button" @click="updateDesign">Update Design</button>
          </div>
        </div>
      </div>
    </div>
    
    <!-- Modal sửa tên sản phẩm -->
    <div v-if="showEditNameModal" class="edit-name-modal">
      <div class="edit-name-modal-content">
        <div class="edit-name-modal-header">
          <h3>Edit Product Name</h3>
          <button class="close-button" @click="showEditNameModal = false">×</button>
        </div>
        <div class="edit-name-modal-body">
          <div class="form-group">
            <label for="productName">Product Name:</label>
            <input type="text" id="productName" v-model="customProductName" class="form-control" placeholder="Enter new product name" />
          </div>
          <div class="edit-name-modal-actions">
            <button class="btn btn-secondary" @click="showEditNameModal = false">Cancel</button>
            <button class="btn btn-primary" @click="updateProductName">Save</button>
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
              title="See the previous image"
            >
              ←
            </button>
            <button 
              v-if="uploadedImageHistory.length > 1 && currentImageIndex < uploadedImageHistory.length - 1" 
              @click="showNextImage" 
              class="next-image-button"
              title="See the next image"
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
        <h3 class="card-title">CUSTOMIZABLE</h3>
        
        <div class="customizer-tabs">
          <button 
            :class="{'tab-button': true, 'active': activeTab === 'color'}" 
            @click="activeTab = 'color'"
          >
            Color
          </button>
          <button 
            :class="{'tab-button': true, 'active': activeTab === 'image'}" 
            @click="activeTab = 'image'"
          >
            Image
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
                Apply
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
                Choose picture
              </label>
              <div class="image-buttons">
                <button class="text-button apply-text" @click="applyImageToMesh" :disabled="!selectedImage" title="Áp dụng ảnh vào phần đã chọn">
                  Apply
                </button>
                <button class="text-button remove-text" @click="removeImagePreview" title="Xóa ảnh đã chọn">
                  Delete
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
                    Move to Image 
                  </button>
                  <button class="text-button remove-text" @click="removeAIImage" title="Xóa ảnh đã tạo">
                    Delete
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
        <h3>Confirm Tab Switch</h3>
        <button class="close-button" @click="showConfirmModal = false">×</button>
      </div>
      <div class="capture-modal-body">
        <p>AI has finished generating the image. Would you like to switch to the image tab to apply it?</p>
        <div class="complete-actions">
          <button class="action-button" @click="showConfirmModal = false">Cancel</button>
          <button class="action-button primary-button" @click="handleConfirmSwitchTab">Switch Tab</button>
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
import { CustomShoeDesign } from '~/server/designUp-service'
import { getManufacturerById, getManufacturerAll } from '@/server/manuService-service.js'
import { getMyCustomById } from '~/server/myCustom-service'
import axios from 'axios'

// Container reference and state
const container = ref(null)
const surcharge = ref(0)
const isCanvasExpanded = ref(false)
const router = useRouter()
const route = useRoute()

// Modal states
const showCaptureModal = ref(false)
const showCompleteModal = ref(false)
const showSurchargeInfo = ref(false)
const showEditNameModal = ref(false)
const showConfirmModal = ref(false)
const selectedAngleIndex = ref(0)
const customProductName = ref('')
const generatedImageUrl = ref('')

// Manufacturer and pricing
const apiSurcharges = ref([])
const basePrice = ref(0)
const manufacturerList = ref([])
const selectedManufacturer = ref(null)

// Template and design data
const templateId = route.params.id
const templateData = ref(null)
const model3DUrl = ref('')
const description = ref('')
const color = ref('')
const gender = ref('')
const isAvailable = ref(false)
const createdAt = ref('')
const updatedAt = ref('')
const previewImageUrl = ref('')

// Edit mode state
const isEditing = ref(false)
const editingDesign = ref(null)

// Three.js objects
let scene, camera, renderer, controls, model
const materials = reactive({})
const customTextures = reactive({})
const componentMeshes = reactive({}) // Define componentMeshes as a reactive object

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
const uploadedImageHistory = reactive([])
const currentImageIndex = ref(-1)

// AI Generate state
const aiPrompt = ref('')
const generatedAIImage = ref(null)
const isGenerating = ref(false)
const aiError = ref(null)

// Part colors and textures
const partColors = reactive({
  Accent: '#ffffff',
  Base: '#ffffff',
  Lace: '#ffffff',
  Sole: '#ffffff',
  Details: '#ffffff',
})

const partTextures = reactive({
  Accent: null,
  Base: null,
  Lace: null,
  Sole: null,
  Details: null,
})

// Components list
const components = reactive([
  { name: 'Base', value: 'Base' },
  { name: 'Details', value: 'Details' },
  { name: 'Sole', value: 'Sole' },
  { name: 'Accent', value: 'Accent' },
  { name: 'Lace', value: 'Lace' }
])

// Part groups
const partGroups = reactive({
  Accent: ['Accent'],
  Details: ['Details'],
  Sole: ['Sole'],
  Base: ['Base'],
  Lace: ['Lace']
})

// Selected component and color
const selectedComponentIndex = ref(0)
const selectedColor = ref('#000000')
const customColorValue = ref('#ffffff')

// Camera position storage
let originalCameraPosition = null

// Capture angles
const captureAngles = reactive([
  { name: 'Back View', preview: null, position: { x: -6, y: 2, z: -6 } },
  { name: 'Left Side', preview: null, position: { x: -4, y: 0, z: 6 } },
  { name: 'Right Side', preview: null, position: { x: 4, y: 0, z: -6 } },
  { name: 'Top View', preview: null, position: { x: 4, y: 8, z: 4 } },
  { name: 'Bottom View', preview: null, position: { x: -4, y: -12, z: 4 } }
])

// Computed properties
const currentManufacturer = computed(() => {
  return manufacturerList.value.find(m => m.id === selectedManufacturer.value) || manufacturerList.value[0]
})

const selectedComponent = computed(() => {
  const selected = components.find((comp, index) => index === selectedComponentIndex.value)
  return selected ? selected.name : null
})

// Modal handlers
const openEditNameModal = () => {
  if (!customProductName.value) {
    customProductName.value = 'Custom Running Shoes'
  }
  showEditNameModal.value = true
}

const updateProductName = () => {
  if (customProductName.value.trim() === '') {
    customProductName.value = 'Custom Running Shoes'
  }
  showEditNameModal.value = false
}

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
  if (!camera || !renderer || !scene || !controls) {
    console.error('Three.js components not initialized properly')
    throw new Error('Three.js components not initialized')
  }

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

const removeImagePreview = () => {
  if (selectedImage.value) {
    if (uploadedImageHistory.length > 0 && currentImageIndex.value >= 0) {
      uploadedImageHistory.splice(currentImageIndex.value, 1)
      
      if (uploadedImageHistory.length > 0) {
        currentImageIndex.value = uploadedImageHistory.length - 1
        const latestImage = uploadedImageHistory[currentImageIndex.value]
        selectedImage.value = latestImage.file
        selectedImageName.value = latestImage.name
        previewImageUrl.value = latestImage.imageUrl
        return
      } else {
        currentImageIndex.value = -1
      }
    }
  }
  
  selectedImage.value = null
  selectedImageName.value = ''
  previewImageUrl.value = ''
  if (imageInput.value) {
    imageInput.value.value = ''
  }
}

const showPreviousImage = () => {
  if (currentImageIndex.value > 0) {
    currentImageIndex.value--
    const img = uploadedImageHistory[currentImageIndex.value]
    selectedImage.value = img.file
    selectedImageName.value = img.name
    previewImageUrl.value = img.imageUrl
  }
}

const showNextImage = () => {
  if (currentImageIndex.value < uploadedImageHistory.length - 1) {
    currentImageIndex.value++
    const img = uploadedImageHistory[currentImageIndex.value]
    selectedImage.value = img.file
    selectedImageName.value = img.name
    previewImageUrl.value = img.imageUrl
  }
}

const generateAIImage = async () => {
  if (!aiPrompt.value.trim()) {
    aiError.value = 'Please enter a prompt'
    return
  }

  isGenerating.value = true
  aiError.value = null

  try {
    const formData = new FormData()
    formData.append('Prompt', aiPrompt.value)
    formData.append('OwnerId', localStorage.getItem('userId'))

    const result = await aiService.generateImage(formData)

    if (result.data && result.data.imageUrl) {
      const timestamp = new Date().getTime()
      const separator = result.data.imageUrl.includes('?') ? '&' : '?'
      const imageUrl = `${result.data.imageUrl}${separator}t=${timestamp}`
      generatedAIImage.value = imageUrl
      
      const response = await fetch(imageUrl)
      const blob = await response.blob()
      const file = new File([blob], 'ai-generated.png', { type: blob.type })

      uploadedImageHistory.push({
        file: file,
        name: 'AI Generated Image',
        imageUrl: imageUrl
      })
      currentImageIndex.value = uploadedImageHistory.length - 1
      
      selectedImage.value = file
      selectedImageName.value = 'AI Generated Image'
      previewImageUrl.value = imageUrl
      
      activeTab.value = 'image'
    } else {
      throw new Error('No image URL received from server')
    }
  } catch (error) {
    console.error('Error generating image:', error)
    aiError.value = 'Error generating image. Please try again.'
  } finally {
    isGenerating.value = false
  }
}

const removeAIImage = () => {
  generatedAIImage.value = null
}

const moveToImageSection = () => {
  if (!generatedAIImage.value) return
  
  try {
    uploadedImageHistory.push({
      file: dataURLtoFile(generatedAIImage.value, 'ai-generated.png'),
      name: 'AI Generated Image',
      imageUrl: generatedAIImage.value
    })
    
    currentImageIndex.value = uploadedImageHistory.length - 1
    
    selectedImage.value = uploadedImageHistory[currentImageIndex.value].file
    selectedImageName.value = 'AI Generated Image'
    previewImageUrl.value = generatedAIImage.value
    
    activeTab.value = 'image'
    
    generatedAIImage.value = null
  } catch (error) {
    console.error('Lỗi khi chuyển ảnh:', error)
  }
}

const handleConfirmSwitchTab = () => {
  showConfirmModal.value = false
  activeTab.value = 'image'
  
  if (generatedImageUrl.value) {
    const file = dataURLtoFile(generatedImageUrl.value, 'ai-generated.png')
    uploadedImageHistory.push({
      file: file,
      name: 'AI Generated Image',
      imageUrl: generatedImageUrl.value
    })
    currentImageIndex.value = uploadedImageHistory.length - 1
    
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
  while(n--) {
    u8arr[n] = bstr.charCodeAt(n)
  }
  return new File([u8arr], filename, {type:mime})
}

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

  animate()
  window.addEventListener('resize', onWindowResize)
}

const loadModel = () => {
  const dracoLoader = new DRACOLoader()
  dracoLoader.setDecoderPath('https://www.gstatic.com/draco/versioned/decoders/1.5.5/')
  const loader = new GLTFLoader()
  loader.setDRACOLoader(dracoLoader)

  const modelPath = isEditing.value && editingDesign.value?.templateUrl
    ? editingDesign.value.templateUrl
    : model3DUrl.value || currentManufacturer.value?.modelPath || ''
  
  if (!modelPath || typeof modelPath !== 'string' || !modelPath.lastIndexOf) {
    alert('No valid 3D file path found!')
    return
  }
  
  loader.load(
    modelPath,
    (gltf) => onModelLoaded(gltf),
    (xhr) => null,
    (error) => alert('Error loading 3D model')
  )
}

const onModelLoaded = (gltf) => {
  if (model) {
    scene.remove(model);
  }

  model = gltf.scene;
  model.scale.set(40, 40, 40);
  model.position.set(0, -2, 0);
  model.rotation.y = Math.PI / 4;

  const foundMeshes = [];
  const meshMaterialMap = {};
  const originalMaterials = {};

  // Clear previous componentMeshes
  Object.keys(componentMeshes).forEach(key => delete componentMeshes[key]);

  model.traverse((node) => {
    if (node.isMesh) {
      foundMeshes.push(node.name);
      node.castShadow = true;
      node.receiveShadow = true;

      if (!node.material) {
        node.material = new THREE.MeshStandardMaterial({ color: 0x808080 });
      }

      materials[node.name] = node.material.clone();
      meshMaterialMap[node.name] = node.material;
      originalMaterials[node.name] = node.material.clone();

      if (node.material.type === 'MeshStandardMaterial') {
        node.material.metalness = node.material.metalness !== undefined ? node.material.metalness : 0.3;
        node.material.roughness = node.material.roughness !== undefined ? node.material.roughness : 0.4;
        node.material.needsUpdate = true;
      }

      if (!node.material.map && !node.material.color) {
        node.material.color = new THREE.Color(0xffffff);
        node.material.needsUpdate = true;
      }
    }
  });

  // Log all found meshes
  console.log('Found meshes in the 3D model:', foundMeshes);

  // Map meshes to components based on partGroups and Lace logic
  foundMeshes.forEach(meshName => {
    const component = Object.keys(partGroups).find(comp => 
      partGroups[comp].some(part => meshName.toLowerCase().includes(part.toLowerCase()))
    ) || (meshName.toLowerCase().includes('lace') || 
          meshName.toLowerCase().includes('shoelace') || 
          meshName.toLowerCase().includes('string') || 
          meshName.toLowerCase().includes('cord') ? 'Lace' : null);

    if (component) {
      if (!componentMeshes[component]) {
        componentMeshes[component] = [];
      }
      componentMeshes[component].push({ name: meshName });
    }
  });

  // Log the components and their associated meshes
  console.log('Components and their meshes:', componentMeshes);

  if (isEditing.value && editingDesign.value?.designData) {
    const designData = editingDesign.value.designData;
    Object.keys(designData.colors).forEach(part => {
      if (componentMeshes[part]) {
        componentMeshes[part].forEach(({ name }) => {
          if (materials[name]) {
            materials[name].color = new THREE.Color(designData.colors[part]);
            materials[name].needsUpdate = true;
          }
        });
        partColors[part] = designData.colors[part];
      }
    });

    Object.keys(designData.textures).forEach(part => {
      if (designData.textures[part].type === 'image' && designData.imagesData[part]) {
        const textureLoader = new THREE.TextureLoader();
        textureLoader.load(designData.imagesData[part], (texture) => {
          texture.repeat.set(textureParams.repeatX * textureParams.scale, textureParams.repeatY * textureParams.scale);
          texture.offset.set(textureParams.offsetX, textureParams.offsetY);
          texture.rotation = textureParams.rotation;
          texture.wrapS = texture.wrapT = THREE.RepeatWrapping;
          texture.needsUpdate = true;
          partTextures[part] = texture;
          if (componentMeshes[part]) {
            componentMeshes[part].forEach(({ name }) => {
              if (materials[name]) {
                materials[name].map = texture;
                materials[name].transparent = true;
                materials[name].needsUpdate = true;
              }
            });
          }
        });
      }
    });

    if (designData.textureParams) {
      Object.assign(textureParams, designData.textureParams);
    }
  }

  // Apply initial colors and textures
  Object.keys(partColors).forEach((partName) => {
    const meshes = componentMeshes[partName] || [];
    if (meshes.length === 0) return;

    const texture = partTextures[partName];
    if (texture) {
      texture.repeat.set(textureParams.repeatX * textureParams.scale, textureParams.repeatY * textureParams.scale);
      texture.offset.set(textureParams.offsetX, textureParams.offsetY);
      texture.rotation = textureParams.rotation;
      texture.wrapS = texture.wrapT = THREE.RepeatWrapping;
      texture.needsUpdate = true;
    }

    meshes.forEach(({ name }) => {
      const originalMaterial = originalMaterials[name] || new THREE.MeshStandardMaterial({ color: 0xffffff });
      
      if (materials[name]) {
        materials[name].color = new THREE.Color(partColors[partName]);
        materials[name].map = texture || null;
        materials[name].transparent = !!texture;
        materials[name].metalness = 0.3;
        materials[name].roughness = 0.4;
        materials[name].needsUpdate = true;
      } else {
        materials[name] = originalMaterial.clone();
        materials[name].color = new THREE.Color(partColors[partName]);
        materials[name].map = texture || null;
        materials[name].transparent = !!texture;
        materials[name].metalness = 0.3;
        materials[name].roughness = 0.4;
        materials[name].needsUpdate = true;
      }

      model.traverse((node) => {
        if (node.isMesh && node.name === name) {
          node.material = materials[node.name];
          node.material.needsUpdate = true;
        }
      });
    });
  });

  checkPartGroups(foundMeshes);
  scene.add(model);
  renderer.render(scene, camera);
};

const checkPartGroups = (foundMeshes) => {
  const missingMeshes = []
  for (const groupName in partGroups) {
    partGroups[groupName].forEach(partName => {
      if (!foundMeshes.some(mesh => mesh.toLowerCase().includes(partName.toLowerCase()))) {
        missingMeshes.push(partName)
      }
    })
  }
  if (missingMeshes.length > 0) {
    console.warn('Missing meshes for components:', missingMeshes)
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
  // Logic if needed when component changes
}

const handleCustomColorChange = () => {
  selectedColor.value = customColorValue.value
}

const applyCustomColor = () => {
  if (!model) {
    console.error('Model is not loaded. Cannot apply color.')
    return
  }

  const selectedPart = components[selectedComponentIndex.value].value;
  const meshes = componentMeshes[selectedPart] || [];

  if (meshes.length === 0) {
    console.warn(`No meshes found for component: ${selectedPart}`);
    return;
  }

  meshes.forEach(({ name }) => {
    if (!customTextures[selectedPart]) {
      customTextures[selectedPart] = {
        originalMap: materials[name] ? materials[name].map : null,
        originalColor: materials[name] && materials[name].color 
          ? materials[name].color.clone() 
          : new THREE.Color('#ffffff')
      };
    }

    const newMaterial = new THREE.MeshStandardMaterial({
      color: new THREE.Color(customColorValue.value),
      map: partTextures[selectedPart] || null,
      transparent: !!partTextures[selectedPart],
      side: selectedPart === 'Lace' ? THREE.DoubleSide : THREE.FrontSide,
      metalness: 0.3,
      roughness: 0.4
    });

    materials[name] = newMaterial;
    materials[name].needsUpdate = true;

    model.traverse((node) => {
      if (node.isMesh && node.name === name) {
        node.material = newMaterial;
        node.material.needsUpdate = true;
      }
    });

    partColors[selectedPart] = customColorValue.value;
    if (!partTextures[selectedPart]) {
      partTextures[selectedPart] = null;
      delete customTextures[selectedPart].texture;
    }
  });

  renderer.render(scene, camera);
  calculateSurcharge();
};

const applyImageToMesh = () => {
  if (!selectedImage.value) {
    alert('Please select an image before applying');
    return;
  }

  const selectedPart = components[selectedComponentIndex.value].value;
  const meshes = componentMeshes[selectedPart] || [];

  if (meshes.length === 0) {
    console.warn(`No meshes found for component: ${selectedPart}`);
    return;
  }

  const imageUrl = previewImageUrl.value;
  const textureLoader = new THREE.TextureLoader();
  textureLoader.load(
    imageUrl,
    (texture) => {
      texture.anisotropy = renderer.capabilities.getMaxAnisotropy();
      texture.wrapS = texture.wrapT = THREE.RepeatWrapping;
      texture.flipY = false;
      texture.encoding = THREE.sRGBEncoding;
      texture.repeat.set(textureParams.repeatX * textureParams.scale, textureParams.repeatY * textureParams.scale);
      texture.offset.set(textureParams.offsetX, textureParams.offsetY);
      texture.rotation = textureParams.rotation;
      texture.needsUpdate = true;

      meshes.forEach(({ name }) => {
        const newMaterial = new THREE.MeshStandardMaterial({
          map: texture,
          color: new THREE.Color(textureParams.brightness, textureParams.brightness, textureParams.brightness),
          transparent: true,
          side: selectedPart === 'Lace' ? THREE.DoubleSide : THREE.FrontSide,
          metalness: 0.3,
          roughness: 0.4
        });

        if (!customTextures[selectedPart]) {
          customTextures[selectedPart] = {
            originalMap: materials[name] ? materials[name].map : null,
            originalColor: materials[name] && materials[name].color 
              ? materials[name].color.clone() 
              : new THREE.Color('#ffffff'),
            texture,
            imageData: imageUrl
          };
        } else {
          customTextures[selectedPart].texture = texture;
          customTextures[selectedPart].imageData = imageUrl;
        }

        materials[name] = newMaterial;
        partTextures[selectedPart] = texture;

        model.traverse((node) => {
          if (node.isMesh && node.name === name) {
            node.material = newMaterial;
            node.material.needsUpdate = true;
          }
        });
      });

      renderer.render(scene, camera);
      calculateSurcharge();
    },
    undefined,
    (error) => {
      alert('An error occurred while loading the image, please try again');
    }
  );
};

const applyTextToMesh = () => {
  if (!customText.value.trim()) {
    alert('Please enter text before applying');
    return;
  }

  const selectedPart = components[selectedComponentIndex.value].value;
  const meshes = componentMeshes[selectedPart] || [];

  if (meshes.length === 0) {
    console.warn(`No meshes found for component: ${selectedPart}`);
    return;
  }

  const canvas = document.createElement('canvas');
  canvas.width = 1024;
  canvas.height = 1024;
  const context = canvas.getContext('2d');

  context.fillStyle = '#ffffff';
  context.fillRect(0, 0, canvas.width, canvas.height);

  context.save();
  context.translate(canvas.width / 2, canvas.height / 2);
  context.rotate(Math.PI);
  context.scale(-1, 1);
  context.translate(-canvas.width / 2, -canvas.height / 2);

  const textLength = customText.value.length;
  const fontSize = Math.min(150, 600 / Math.max(1, textLength / 3));
  context.font = `bold ${fontSize}px Arial, sans-serif`;
  context.strokeStyle = 'black';
  context.lineWidth = fontSize / 8;
  context.textAlign = 'center';
  context.textBaseline = 'middle';
  context.strokeText(customText.value, canvas.width / 2, canvas.height / 2);
  context.fillStyle = '#ffffff';
  context.shadowColor = 'rgba(0, 0, 0, 0.7)';
  context.shadowBlur = 3;
  context.shadowOffsetX = 1;
  context.shadowOffsetY = 1;
  context.fillText(customText.value, canvas.width / 2, canvas.height / 2);
  context.shadowColor = 'transparent';
  context.fillText(customText.value, canvas.width / 2, canvas.height / 2);

  context.restore();

  const texture = new THREE.CanvasTexture(canvas);
  texture.anisotropy = renderer.capabilities.getMaxAnisotropy();
  texture.wrapS = texture.wrapT = THREE.RepeatWrapping;
  texture.repeat.set(1, 1);
  texture.needsUpdate = true;

  meshes.forEach(({ name }) => {
    const newMaterial = new THREE.MeshStandardMaterial({
      map: texture,
      transparent: true,
      side: selectedPart === 'Lace' ? THREE.DoubleSide : THREE.FrontSide,
      metalness: 0.3,
      roughness: 0.4
    });

    if (!customTextures[selectedPart]) {
      customTextures[selectedPart] = {
        originalMap: materials[name] ? materials[name].map : null,
        originalColor: materials[name] && materials[name].color 
          ? materials[name].color.clone() 
          : new THREE.Color('#ffffff'),
        texture
      };
    } else {
      customTextures[selectedPart].texture = texture;
    }

    materials[name] = newMaterial;
    partTextures[selectedPart] = texture;

    model.traverse((node) => {
      if (node.isMesh && node.name === name) {
        node.material = newMaterial;
        node.material.needsUpdate = true;
      }
    });
  });

  renderer.render(scene, camera);
  calculateSurcharge();
};

const removeTextFromMesh = () => {
  const selectedPart = components[selectedComponentIndex.value].value;
  const meshes = componentMeshes[selectedPart] || [];

  if (meshes.length === 0) {
    console.warn(`No meshes found for component: ${selectedPart}`);
    return;
  }

  meshes.forEach(({ name }) => {
    if (customTextures[selectedPart]) {
      const newMaterial = new THREE.MeshStandardMaterial({
        map: customTextures[selectedPart].originalMap,
        color: customTextures[selectedPart].originalColor,
        transparent: customTextures[selectedPart].originalMap ? true : false,
        side: selectedPart === 'Lace' ? THREE.DoubleSide : THREE.FrontSide,
        metalness: 0.3,
        roughness: 0.4
      });

      materials[name] = newMaterial;
      materials[name].needsUpdate = true;

      model.traverse((node) => {
        if (node.isMesh && node.name === name) {
          node.material = newMaterial;
          node.material.needsUpdate = true;
        }
      });

      partTextures[selectedPart] = null;
      delete customTextures[selectedPart];
    }
  });

  if (imageInput.value) {
    imageInput.value.value = '';
  }
  if (activeTab.value === 'image') {
    if (previewImageUrl.value) {
      previewImageUrl.value = '';
    }
    selectedImage.value = null;
    selectedImageName.value = '';
  }

  renderer.render(scene, camera);
  calculateSurcharge();
};

const saveAsDraft = () => {
  showCompleteModal.value = false;
  calculateSurcharge();
  
  let serviceIds = [];
  let textureIds = [];

  const productData = {
    id: isEditing.value ? editingDesign.value.id : Date.now(),
    name: customProductName.value || 'Custom Running Shoes (Draft)',
    templateId: route.params.id,
    manufacturerId: selectedManufacturer.value || 1,
    price: basePrice.value,
    surcharge: surcharge.value,
    image: captureAngles[1].preview,
    textureIds: textureIds,
    ServiceIds: serviceIds,
    designData: {
      colors: {},
      textures: {},
      imagesData: {},
      customText: customText.value,
      textureParams: { ...textureParams },
      timestamp: new Date().toISOString(),
      manufacturerId: selectedManufacturer.value || 1,
      previewImages: captureAngles.map(angle => angle.preview) 
    },
    previewImages: []
  };

  for (const comp of components) {
    const partName = comp.value;
    const meshes = componentMeshes[partName] || [];
    meshes.forEach(({ name }) => {
      if (materials[name]) {
        const hexColor = '#' + materials[name].color.getHexString();
        productData.designData.colors[partName] = hexColor;
        if (hexColor.toLowerCase() !== '#ffffff') {
          const colorService = apiSurcharges.value.find(
            s => s.component === partName.toLowerCase() && s.type === 'colorapplication'
          );
          if (colorService) {
            productData.ServiceIds.push(colorService.id);
          }
        }
        if (customTextures[partName]) {
          const textureType = customTextures[partName].texture instanceof THREE.CanvasTexture ? 'text' : 'image';
          productData.designData.textures[partName] = {
            type: textureType,
            textContent: customText.value
          };
          if (textureType === 'image' && customTextures[partName].imageData) {
            productData.designData.imagesData[partName] = customTextures[partName].imageData;
            const imageService = apiSurcharges.value.find(
              s => s.component === partName.toLowerCase() && s.type === 'imageapplication'
            );
            if (imageService) {
              productData.ServiceIds.push(imageService.id);
            }
          }
        }
      }
    });
  }

  productData.ServiceIds = Array.from(new Set(productData.ServiceIds)).map(Number);
  productData.textureIds = textureIds.map(Number);

  productData.previewImages = captureAngles.map((angle, idx) => {
    if (angle.preview && angle.preview.startsWith('data:image')) {
      const arr = angle.preview.split(',');
      const mime = arr[0].match(/:(.*?);/)[1];
      const bstr = atob(arr[1]);
      let n = bstr.length;
      const u8arr = new Uint8Array(n);
      while(n--) u8arr[n] = bstr.charCodeAt(n);
      return new File([u8arr], `preview_${idx}.png`, { type: mime });
    }
    return angle.preview;
  });

  CustomShoeDesign(productData)
    .then(response => {
      console.log('Draft saved successfully:', response);
      localStorage.removeItem('editingDesign');
      alert('Draft saved successfully!');
      window.location.href = '/mycustomPage';
    })
    .catch(error => {
      console.error('Error saving draft:', error);
      alert('An error occurred while saving the draft. Please try again.');
    });
};

const updateDesign = async () => {
  try {
    console.log('Starting updateDesign process...');
    await captureAllAngles();

    console.log('Capture Angles Previews:', captureAngles.map((angle, index) => ({
      name: angle.name,
      index,
      previewLength: angle.preview ? angle.preview.length : 'null',
      previewSnippet: angle.preview ? angle.preview.substring(0, 50) + '...' : 'null',
    })));

    const validPreviews = captureAngles.every((angle, index) => {
      if (!angle.preview || !angle.preview.startsWith('data:image')) {
        console.warn(`Invalid preview for angle ${index} (${angle.name}):`, angle.preview);
        return false;
      }
      return true;
    });

    if (!validPreviews || captureAngles.length === 0) {
      throw new Error('No valid preview images generated. Please try capturing angles again.');
    }

    let serviceIds = [];
    let textureIds = [];

    const designData = {
      colors: {},
      textures: {},
      imagesData: {},
      customText: customText.value,
      textureParams: { ...textureParams },
      timestamp: new Date().toISOString(),
      manufacturerId: selectedManufacturer.value || 1,
    };

    for (const comp of components) {
      const partName = comp.value;
      const meshes = componentMeshes[partName] || [];
      meshes.forEach(({ name }) => {
        if (materials[name]) {
          const hexColor = '#' + materials[name].color.getHexString();
          designData.colors[partName] = hexColor;
          if (hexColor.toLowerCase() !== '#ffffff') {
            const colorService = apiSurcharges.value.find(
              (s) => s.component === partName.toLowerCase() && s.type === 'colorapplication'
            );
            if (colorService) {
              serviceIds.push(colorService.id);
            }
          }
          if (customTextures[partName]) {
            const textureType = customTextures[partName].texture instanceof THREE.CanvasTexture ? 'text' : 'image';
            designData.textures[partName] = {
              type: textureType,
              textContent: customText.value,
            };
            if (textureType === 'image' && customTextures[partName].imageData) {
              designData.imagesData[partName] = customTextures[partName].imageData;
              const imageService = apiSurcharges.value.find(
                (s) => s.component === partName.toLowerCase() && s.type === 'imageapplication'
              );
              if (imageService) {
                serviceIds.push(imageService.id);
              }
            }
          }
        }
      });
    }

    serviceIds = Array.from(new Set(serviceIds)).map(Number);
    textureIds = Array.from(new Set(textureIds)).map(Number);

    const designDataJson = JSON.stringify(designData, null, 2);
    const designDataBlob = new Blob([designDataJson], { type: 'application/json' });

    const formData = new FormData();
    formData.append('UserId', localStorage.getItem('userId'));
    formData.append('id', editingDesign.value.id);
    formData.append('CustomShoeDesignTemplateId', editingDesign.value.templateId);
    formData.append('Name', customProductName.value || 'Custom Running Shoes');
    formData.append('Description', description.value || 'stylish comfort that keeps you moving with confidence');
    formData.append('DesignData', designDataBlob);
    formData.append('DesignerMarkup', '0');

    textureIds.forEach((id) => formData.append('TextureIds', id));
    serviceIds.forEach((id) => formData.append('ServiceIds', id));

    const appendedImages = [];
    captureAngles.forEach((angle, index) => {
      if (angle.preview && angle.preview.startsWith('data:image')) {
        const arr = angle.preview.split(',');
        const mime = arr[0].match(/:(.*?);/)[1];
        const bstr = atob(arr[1]);
        let n = bstr.length;
        const u8arr = new Uint8Array(n);
        while(n--) u8arr[n] = bstr.charCodeAt(n);
        const file = new File([u8arr], `preview_${index}.png`, { type: mime });
        formData.append('CustomShoeDesignPreviewImages', file);
        appendedImages.push({
          name: angle.name,
          index,
          fileSize: file.size,
          fileType: file.type,
        });
        console.log(`Appended preview image for ${angle.name} (index ${index})`);
      }
    });

    console.log('Appended Preview Images:', appendedImages);

    if (appendedImages.length !== captureAngles.length) {
      throw new Error(`Mismatch in number of appended images: expected ${captureAngles.length}, got ${appendedImages.length}`);
    }

    console.log('Sending request to /api/CustomShoeDesign...');
    const response = await axios.put(`/api/CustomShoeDesign`, formData, {
      headers: { 'Content-Type': 'multipart/form-data' }
    });

    if (response.status === 200 || response.status === 204) {
      localStorage.removeItem('editingDesign');
      alert('Design updated successfully!');
      router.push('/mycustomPage');
    } else {
      throw new Error('Unexpected response status');
    }
  } catch (error) {
    console.error('Error updating design:', error);
    alert('An error occurred while updating the design. Please try again.');
  } finally {
    showCompleteModal.value = false;
  }
};

const toggleCanvasSize = () => {
  isCanvasExpanded.value = !isCanvasExpanded.value;
  setTimeout(() => {
    if (renderer && container.value) {
      onWindowResize();
    }
  }, 300);
};

const updateTextureParameters = () => {
  const selectedPart = components[selectedComponentIndex.value].value;
  const meshes = componentMeshes[selectedPart] || [];

  meshes.forEach(({ name }) => {
    if (partTextures[selectedPart] && materials[name]) {
      const texture = partTextures[selectedPart];
      texture.repeat.set(textureParams.repeatX * textureParams.scale, textureParams.repeatY * textureParams.scale);
      texture.offset.set(textureParams.offsetX, textureParams.offsetY);
      texture.rotation = textureParams.rotation;
      texture.needsUpdate = true;
      
      if (customTextures[selectedPart] && customTextures[selectedPart].imageData) {
        materials[name].color.set(new THREE.Color(textureParams.brightness, textureParams.brightness, textureParams.brightness));
      }
      
      materials[name].needsUpdate = true;
      materials[name].metalness = 0.3;
      materials[name].roughness = 0.4;

      model.traverse((node) => {
        if (node.isMesh && node.name === name) {
          node.material = materials[name];
          node.material.needsUpdate = true;
        }
      });
    }
  });
  
  renderer.render(scene, camera);
};

const findAllLaceMeshes = () => {
  if (!model) return [];
  
  const laceMeshes = [];
  model.traverse((node) => {
    if (node.isMesh && 
       (node.name === 'Lace' || 
        node.name.toLowerCase().includes('lace') || 
        node.name.toLowerCase().includes('shoelace') || 
        node.name.toLowerCase().includes('string') ||
        node.name.toLowerCase().includes('cord'))) {
      laceMeshes.push(node);
    }
  });
  
  return laceMeshes;
};

const fetchSurchargeData = async (manufacturerId) => {
  try {
    const res = await getManufacturerById(manufacturerId);
    if (res && res.data) {
      apiSurcharges.value = res.data.services || [];
    }
  } catch (error) {
    apiSurcharges.value = [];
  }
};

const calculateSurcharge = () => {
  let totalSurcharge = 0;
  for (const comp of components) {
    const partName = comp.value;
    let hasColor = false;
    let hasImage = false;
    const meshes = componentMeshes[partName] || [];
    meshes.forEach(({ name }) => {
      if (materials[name]) {
        const hexColor = '#' + materials[name].color.getHexString();
        if (hexColor.toLowerCase() !== '#ffffff') hasColor = true;
        if (customTextures[partName]) {
          const textureType = customTextures[partName].texture instanceof THREE.CanvasTexture ? 'text' : 'image';
          if (textureType === 'image') hasImage = true;
        }
      }
    });
    if (hasColor) {
      const colorFee = apiSurcharges.value.find(
        s => s.component === partName.toLowerCase() && s.type === 'colorapplication'
      );
      if (colorFee) totalSurcharge += colorFee.currentAmount;
    }
    if (hasImage) {
      const imageFee = apiSurcharges.value.find(
        s => s.component === partName.toLowerCase() && s.type === 'imageapplication'
      );
      if (imageFee) totalSurcharge += imageFee.currentAmount;
    }
  }
  surcharge.value = Math.round(totalSurcharge);
};

const handleManufacturerChange = async () => {
  await fetchSurchargeData(selectedManufacturer.value);
  calculateSurcharge();
  loadModel();
};

const formatPrice = (price) => {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(price || 0);
};

watch(showEditNameModal, (newValue) => {
  if (newValue) {
    document.body.style.overflow = 'hidden';
  } else {
    document.body.style.overflow = '';
  }
});

watch(textureParams, updateTextureParameters, { deep: true });

onMounted(async () => {
  axios.defaults.baseURL = 'https://fcspwebapi20250527114117.azurewebsites.net';

  const urlParams = new URLSearchParams(window.location.search);
  isEditing.value = urlParams.get('edit') === 'true';
  const editId = route.params.id;

  if (isEditing.value) {
    const storedDesign = localStorage.getItem('editingDesign');
    if (storedDesign) {
      try {
        editingDesign.value = JSON.parse(storedDesign);
        
        customProductName.value = editingDesign.value.name || 'Custom Running Shoes';
        basePrice.value = editingDesign.value.price || 0;
        model3DUrl.value = editingDesign.value.templateUrl || '';
        selectedManufacturer.value = editingDesign.value.manufacturerId || null;
        previewImageUrl.value = editingDesign.value.previewImages?.[0] || '';
        
        if (editingDesign.value.previewImages?.length) {
          captureAngles.forEach((angle, index) => {
            if (editingDesign.value.previewImages[index]) {
              angle.preview = editingDesign.value.previewImages[index];
            }
          });
        }
      } catch (e) {
        console.error('Error parsing editingDesign:', e);
        alert('Unable to load design data for editing.');
      }
    } else {
      alert('No design data found for editing.');
    }
  }

  let response;
  if (isEditing.value) {
    response = await getMyCustomById(editId);
    if (response && response.data) response = response.data;
  } else {
    response = await getTemplateById(editId);
  }
  
  if (response) {
    templateData.value = response;
    if (!isEditing.value) {
      customProductName.value = response.name || '';
      previewImageUrl.value = response.previewImageUrl || '';
      basePrice.value = response.basePrice || response.price || 0;
      model3DUrl.value = response.model3DUrl || response.templateUrl || '';
      description.value = response.description || 'stylish comfort that keeps you moving with confidence';
      color.value = response.color || '';
      gender.value = response.gender || '';
      isAvailable.value = response.isAvailable || false;
      createdAt.value = response.createdAt || '';
      updatedAt.value = response.updatedAt || '';
    }
  }

  const res = await getManufacturerAll();
  if (res && res.data && Array.isArray(res.data)) {
    manufacturerList.value = res.data;
    if (!selectedManufacturer.value && res.data.length > 0) {
      selectedManufacturer.value = res.data[0].id;
    }
    await fetchSurchargeData(selectedManufacturer.value);
    calculateSurcharge();
  }

  initThree();
  loadModel();
});

onBeforeUnmount(() => {
  window.removeEventListener('resize', onWindowResize);
  if (model) {
    scene.remove(model);
    model.traverse((node) => {
      if (node.isMesh) {
        if (node.geometry) node.geometry.dispose();
        if (node.material) node.material.dispose();
      }
    });
  }
  if (renderer) {
    renderer.dispose();
    if (container.value) container.value.removeChild(renderer.domElement);
  }
  if (controls) controls.dispose();
  if (scene) scene.clear();
  if (previewImageUrl.value) URL.revokeObjectURL(previewImageUrl.value);
});
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
  background: linear-gradient(135deg, #AAAAAA, #888888);
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
  background: linear-gradient(135deg, #888888, #666666);
  transform: translateY(-2px);
  box-shadow: 0 4px 8px rgba(170, 170, 170, 0.3);
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
  border-color: #AAAAAA;
  box-shadow: 0 0 0 3px rgba(170, 170, 170, 0.15);
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
  background: linear-gradient(135deg, #AAAAAA, #888888);
  color: white;
}

.edit-name-modal-actions .btn-primary:hover {
  background: linear-gradient(135deg, #888888, #666666);
  box-shadow: 0 5px 15px rgba(170, 170, 170, 0.3);
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
  background-color: #007bff;
  color: white;
  border-color: #007bff;
}

.apply-text:hover {
  background-color: #0056b3;
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
  background: #AAAAAA;
  cursor: pointer;
  box-shadow: 0 0 0 2px rgba(170, 170, 170, 0.3);
}

.slider::-moz-range-thumb {
  width: 12px;
  height: 12px;
  border-radius: 50%;
  background: #AAAAAA;
  cursor: pointer;
  box-shadow: 0 0 0 2px rgba(170, 170, 170, 0.3);
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
  border-color: #AAAAAA;
  box-shadow: 0 0 0 3px rgba(170, 170, 170, 0.15);
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
  background-color: #AAAAAA;
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
  background: #AAAAAA;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: background 0.3s;
}

.generate-button:not(:disabled):hover {
  background: #888888;
}

.generate-button:disabled {
  background: #ccc;
  cursor: not-allowed;
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