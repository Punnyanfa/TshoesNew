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
import { RGBELoader } from 'three/examples/jsm/loaders/RGBELoader.js' // Thêm RGBELoader để tải HDR environment map

// Container reference
const container = ref(null)

// State for canvas size
const isCanvasExpanded = ref(false)

// State for modals
const showCaptureModal = ref(false)
const showCompleteModal = ref(false)
const selectedAngleIndex = ref(0)
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
const customColorValue = ref('#ff0000')
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
    controls.target.set(0, 1.5, 0)
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
  captureAllAngles().then(() => {
    showCompleteModal.value = true
  })
}

const addToCart = () => {
  showCompleteModal.value = false
  alert('Sản phẩm đã được thêm vào giỏ hàng thành công!')
  window.location.href = '/customPage'
}

const saveAsDraft = () => {
  showCompleteModal.value = false
  const designData = {
    productName: 'Adidas Running Shoes (Tùy chỉnh)',
    price: '2.500.000 ₫',
    components: {},
    textureParams: { ...textureParams },
    customText: customText.value,
    cameraPosition: camera ? { x: camera.position.x, y: camera.position.y, z: camera.position.z } : null,
    timestamp: new Date().toISOString()
  }

  for (const comp of components) {
    const partName = comp.value
    if (materials[partName]) {
      designData.components[partName] = {
        name: comp.name,
        color: '#' + materials[partName].color.getHexString(),
        hasTexture: !!materials[partName].map
      }
      if (customTextures[partName]) {
        designData.components[partName].textureInfo = {
          type: customTextures[partName].texture instanceof THREE.CanvasTexture ? 'text' : 'image',
          textContent: customText.value
        }
      }
    }
  }

  const jsonString = JSON.stringify(designData, null, 2)
  const blob = new Blob([jsonString], { type: 'application/json' })
  const url = URL.createObjectURL(blob)
  const link = document.createElement('a')
  link.href = url
  link.download = 'adidas-custom-design.json'
  document.body.appendChild(link)
  link.click()
  document.body.removeChild(link)
  URL.revokeObjectURL(url)
  alert('Thiết kế đã được lưu vào bản nháp và tải xuống thành công!')
}

// Three.js initialization
const initThree = () => {
  scene = new THREE.Scene()
  scene.background = new THREE.Color(0xf0f0f0)

  // Camera setup
  camera = new THREE.PerspectiveCamera(50, container.value.clientWidth / container.value.clientHeight, 0.1, 100)
  camera.position.set(2, 0.5, 2)
  camera.lookAt(0, 1, 0)

  renderer = new THREE.WebGLRenderer({ antialias: true, alpha: true, preserveDrawingBuffer: true })
  renderer.setPixelRatio(window.devicePixelRatio)
  renderer.setSize(container.value.clientWidth, container.value.clientHeight)
  renderer.shadowMap.enabled = true
  renderer.shadowMap.type = THREE.PCFSoftShadowMap
  renderer.toneMapping = THREE.ACESFilmicToneMapping
  renderer.toneMappingExposure = 1.2
  container.value.appendChild(renderer.domElement)

  // Load HDR environment map
  const rgbeLoader = new RGBELoader()
  rgbeLoader.load(
    'https://dl.polyhaven.org/file/ph-assets/HDRIs/hdr/1k/urban_street_01_1k.hdr', 
    (texture) => {
      texture.mapping = THREE.EquirectangularReflectionMapping
      scene.environment = texture 
      scene.background = new THREE.Color(0xf0f0f0)
    },
    undefined,
    (error) => console.error('Lỗi khi tải HDR environment map:', error)
  )

  // Lighting setup 
  const ambientLight = new THREE.AmbientLight(0xf0f0f0, 1) // Giảm intensity vì có ánh sáng từ environment map
  scene.add(ambientLight)

  const keyLight = new THREE.DirectionalLight(0xffffff, 0.6) // Giảm intensity
  keyLight.position.set(3, 4, 3)
  keyLight.castShadow = true
  keyLight.shadow.mapSize.width = 2048
  keyLight.shadow.mapSize.height = 2048
  keyLight.shadow.camera.near = 0.5
  keyLight.shadow.camera.far = 50
  keyLight.shadow.bias = -0.0001
  scene.add(keyLight)

  const fillLight = new THREE.DirectionalLight(0xffffff, 0.3)
  fillLight.position.set(-3, 2, -3)
  scene.add(fillLight)

  const backLight = new THREE.DirectionalLight(0xffffff, 0.2)
  backLight.position.set(0, 3, -3)
  scene.add(backLight)

  // Orbit controls
  controls = new OrbitControls(camera, renderer.domElement)
  controls.enableDamping = true
  controls.dampingFactor = 0.05
  controls.rotateSpeed = 0.7
  controls.minDistance = 0.8
  controls.maxDistance = 3
  controls.target.set(0, 0.7, 0)
  controls.maxPolarAngle = Math.PI / 1.8
  controls.minPolarAngle = Math.PI / 8

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
  model.scale.set(7, 7, 7)
  model.position.set(0, -0.3, 0)
  model.rotation.y = Math.PI / 4

  const foundMeshes = []
  const meshMaterialMap = {}
  model.traverse((node) => {
    if (node.isMesh) {
      foundMeshes.push(node.name)
      node.castShadow = true
      node.receiveShadow = true
      if (node.material) {
        meshMaterialMap[node.name] = node.material
      } else {
        node.material = new THREE.MeshStandardMaterial({ color: 0x808080 })
        meshMaterialMap[node.name] = node.material
      }
    }
  })

  console.log('Found meshes:', foundMeshes)
  console.log('Mesh material map:', meshMaterialMap)

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
      roughness: originalMaterial.Roughness || 0.6,
      envMap: scene.environment, // Áp dụng environment map
      envMapIntensity: 1.5 // Tăng cường độ ánh sáng môi trường để mô hình sáng hơn
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
  }
}

const onImageSelected = (event) => {
  const file = event.target.files[0]
  if (file) {
    selectedImage.value = file
    selectedImageName.value = file.name.length > 20 ? file.name.substring(0, 17) + '...' : file.name
    previewImageUrl.value = URL.createObjectURL(file)
  }
}

const applyImageToMesh = () => {
  if (!selectedImage.value) {
    alert('Vui lòng chọn ảnh trước khi áp dụng')
    return
  }

  const selectedPart = components[selectedComponentIndex.value].value
  const partsToUpdate = selectedPart in partGroups ? partGroups[selectedPart] : [selectedPart]

  const imageUrl = URL.createObjectURL(selectedImage.value)
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

      partsToUpdate.forEach((part) => {
        if (materials[part]) {
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
          materials[part].color.set(new THREE.Color(textureParams.brightness, textureParams.brightness, textureParams.brightness))
          materials[part].transparent = true
          materials[part].needsUpdate = true
        }
      })

      renderer.render(scene, camera)
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

  partsToUpdate.forEach((part) => {
    if (materials[part]) {
      const currentColor = materials[part].color.clone()
      context.fillStyle = `#${currentColor.getHexString()}`
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
      materials[part].color.copy(currentColor)
      materials[part].needsUpdate = true
    }
  })

  renderer.render(scene, camera)
}

const removeTextFromMesh = () => {
  const selectedPart = components[selectedComponentIndex.value].value
  const partsToUpdate = selectedPart in partGroups ? partGroups[selectedPart] : [selectedPart]

  partsToUpdate.forEach((part) => {
    if (materials[part] && customTextures[part]) {
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
      URL.revokeObjectURL(previewImageUrl.value)
      previewImageUrl.value = ''
    }
    selectedImage.value = null
    selectedImageName.value = ''
  }

  renderer.render(scene, camera)
}

const updatePartColor = (color) => {
  const selectedPart = components[selectedComponentIndex.value].value
  const partsToUpdate = selectedPart in partGroups ? partGroups[selectedPart] : [selectedPart]

  partsToUpdate.forEach((part) => {
    if (materials[part]) {
      partColors[part] = color
      materials[part].color.set(color)
      materials[part].needsUpdate = true

      model.traverse((node) => {
        if (node.isMesh && node.name.toLowerCase().includes(part.toLowerCase())) {
          node.material = materials[part]
          node.material.needsUpdate = true
        }
      })
    } else {
      console.warn(`Material for ${part} not found`)
    }
  })

  renderer.render(scene, camera)
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
      materials[part].color.set(new THREE.Color(textureParams.brightness, textureParams.brightness, textureParams.brightness))
      materials[part].needsUpdate = true

      model.traverse((node) => {
        if (node.isMesh && node.name.toLowerCase().includes(part.toLowerCase())) {
          node.material = materials[part]
          node.material.needsUpdate = true
        }
      })
    }
  })
}

watch(textureParams, updateTextureParameters, { deep: true })

const handleColorClick = (colorValue) => {
  selectedColor.value = colorValue
  customColorApplied.value = false
  updatePartColor(colorValue)
}

const handleCustomColorChange = (event) => {
  customColorValue.value = event.target.value
}

const applyCustomColor = () => {
  selectedColor.value = customColorValue.value
  customColorApplied.value = true
  updatePartColor(customColorValue.value)
}

const handleComponentChange = () => {
  console.log('Đã chọn component:', components[selectedComponentIndex.value].name)
}

onMounted(() => {
  initThree()
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
  if (scene) {
    if (scene.environment) scene.environment.dispose()
    scene.clear()
  }
  if (previewImageUrl.value) URL.revokeObjectURL(previewImageUrl.value)
})
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