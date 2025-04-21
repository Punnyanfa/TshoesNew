<script setup>
import { ref, reactive, onMounted, onBeforeUnmount, watch } from 'vue'
import * as THREE from 'three'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader'
import { DRACOLoader } from 'three/examples/jsm/loaders/DRACOLoader'
import { RGBELoader } from 'three/examples/jsm/loaders/RGBELoader.js'

// Container reference
const container = ref(null)

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

// Part colors and textures
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
  { name: 'MidSole', value: 'MidSode' },
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

// HDRI list
const HDRIs = reactive([
  { name: 'Kloofendal 48d Partly Cloudy', url: 'https://dl.polyhaven.org/file/ph-assets/HDRIs/hdr/1k/kloofendal_48d_partly_cloudy_puresky_1k.hdr', description: 'Bầu trời ngoài trời với mây nhẹ, ánh sáng tự nhiên, mềm mại, tông màu trung tính.' },
  { name: 'Sunflowers PureSky', url: 'https://dl.polyhaven.org/file/ph-assets/HDRIs/hdr/1k/sunflowers_puresky_1k.hdr', description: 'Ánh sáng mặt trời buổi chiều, tông màu vàng ấm áp, bầu trời trong xanh.' },
  { name: 'Noon Grass PureSky', url: 'https://dl.polyhaven.org/file/ph-assets/HDRIs/hdr/1k/noon_grass_puresky_1k.hdr', description: 'Ánh sáng ban trưa trên cánh đồng cỏ, bầu trời trong, ánh sáng mạnh nhưng tự nhiên.' },
  { name: 'Venus PureSky', url: 'https://dl.polyhaven.org/file/ph-assets/HDRIs/hdr/1k/venus_puresky_1k.hdr', description: 'Ánh sáng hoàng hôn nhẹ, tông màu cam-vàng, bầu trời có mây mỏng.' },
  { name: 'Dank PureSky', url: 'https://dl.polyhaven.org/file/ph-assets/HDRIs/hdr/1k/dank_puresky_1k.hdr', description: 'Bầu trời nhiều mây, ánh sáng dịu, tông màu hơi mát.' },
  { name: 'Studio Small 03', url: 'https://dl.polyhaven.org/file/ph-assets/HDRIs/hdr/1k/studio_small_03_1k.hdr', description: 'Ánh sáng studio, mềm mại và cân bằng, tông màu trung tính.' },
  { name: 'Photo Studio 01', url: 'https://dl.polyhaven.org/file/ph-assets/HDRIs/hdr/1k/photo_studio_01_1k.hdr', description: 'Ánh sáng trong studio chụp ảnh, rất sáng và đều, tông màu trung tính.' },
  { name: 'Syferfontein 0d Clear', url: 'https://dl.polyhaven.org/file/ph-assets/HDRIs/hdr/1k/syferfontein_0d_clear_1k.hdr', description: 'Bầu trời trong xanh, ánh sáng mặt trời mạnh, tông màu sáng và tự nhiên.' },
  { name: 'Brown Photostudio 02', url: 'https://dl.polyhaven.org/file/ph-assets/HDRIs/hdr/1k/brown_photostudio_02_1k.hdr', description: 'Ánh sáng studio với tông màu ấm nhẹ, ánh sáng mềm mại.' },
  { name: 'Urban Street 01', url: 'https://dl.polyhaven.org/file/ph-assets/HDRIs/hdr/1k/urban_street_01_1k.hdr', description: 'Ánh sáng ngoài trời trong khu phố đô thị, bầu trời có mây, ánh sáng tự nhiên với chút bóng.' }
])

// State for selected HDRI
const selectedHDRIIndex = ref(0)

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
  captureAllAngles().then(() => {
    showCompleteModal.value = true
  })
}

const addToCart = () => {
  showCompleteModal.value = false

  // Check if editing an existing design
  const urlParams = new URLSearchParams(window.location.search)
  const isEditing = urlParams.get('edit') === 'true'
  const editId = urlParams.get('id')

  // Create product data with custom design
  const productData = {
    id: isEditing && editId ? parseInt(editId) : Date.now(),
    name: 'Adidas Running Shoes (Tùy chỉnh)',
    price: 2500000,
    image: captureAngles[1].preview,
    designData: {
      colors: {},
      textures: {},
      imagesData: {},
      customText: customText.value,
      textureParams: { ...textureParams },
      selectedHDRI: HDRIs[selectedHDRIIndex.value].name,
      timestamp: new Date().toISOString()
    },
    previewImages: captureAngles.map(angle => angle.preview)
  }

  // Save colors and textures
  for (const comp of components) {
    const partName = comp.value
    if (materials[partName]) {
      productData.designData.colors[partName] = '#' + materials[partName].color.getHexString()

      if (customTextures[partName]) {
        const textureType = customTextures[partName].texture instanceof THREE.CanvasTexture ? 'text' : 'image'
        productData.designData.textures[partName] = {
          type: textureType,
          textContent: customText.value
        }
        if (textureType === 'image' && customTextures[partName].imageData) {
          productData.designData.imagesData[partName] = customTextures[partName].imageData
        }
      }
    }
  }

  // Retrieve current cart from localStorage
  let cart = []
  const savedCart = localStorage.getItem('cart')
  if (savedCart) {
    cart = JSON.parse(savedCart)
  }

  if (isEditing && editId) {
    // Update existing product
    const itemIndex = cart.findIndex(item => item.id === parseInt(editId))
    if (itemIndex !== -1) {
      cart[itemIndex] = productData
    } else {
      cart.push(productData)
    }
    alert('Đã cập nhật thiết kế của sản phẩm trong giỏ hàng!')
  } else {
    // Add new product to cart
    cart.push(productData)
    alert('Sản phẩm đã được thêm vào giỏ hàng thành công!')
  }

  // Save cart to localStorage
  localStorage.setItem('cart', JSON.stringify(cart))

  window.location.href = '/cartcustomPage'
}

const saveAsDraft = () => {
  showCompleteModal.value = false

  // Create draft data
  const draftData = {
    id: Date.now(),
    name: 'Adidas Running Shoes (Nháp)',
    price: 2500000,
    image: captureAngles[1].preview,
    designData: {
      colors: {},
      textures: {},
      imagesData: {},
      customText: customText.value,
      textureParams: { ...textureParams },
      selectedHDRI: HDRIs[selectedHDRIIndex.value].name,
      timestamp: new Date().toISOString(),
      cameraPosition: camera ? { x: camera.position.x, y: camera.position.y, z: camera.position.z } : null
    },
    previewImages: captureAngles.map(angle => angle.preview)
  }

  // Save colors and textures
  for (const comp of components) {
    const partName = comp.value
    if (materials[partName]) {
      draftData.designData.colors[partName] = '#' + materials[partName].color.getHexString()
      if (customTextures[partName]) {
        const textureType = customTextures[partName].texture instanceof THREE.CanvasTexture ? 'text' : 'image'
        draftData.designData.textures[partName] = {
          type: textureType,
          textContent: customText.value
        }
        if (textureType === 'image' && customTextures[partName].imageData) {
          draftData.designData.imagesData[partName] = customTextures[partName].imageData
        }
      }
    }
  }

  // Save to localStorage
  let drafts = []
  const savedDrafts = localStorage.getItem('designDrafts')
  if (savedDrafts) {
    drafts = JSON.parse(savedDrafts)
  }
  drafts.push(draftData)
  localStorage.setItem('designDrafts', JSON.stringify(drafts))

  // Also save as JSON file for download
  const jsonString = JSON.stringify(draftData, null, 2)
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
  window.location.href = '/mycustomPage'
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
  renderer.toneMapping = THREE.ACESFilmicToneMapping
  renderer.toneMappingExposure = 1.2
  container.value.appendChild(renderer.domElement)

  // Load initial HDRI
  loadHDRI(HDRIs[selectedHDRIIndex.value].url)

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
  controls.maxDistance = 3
  controls.target.set(0, 0.5, 0)
  controls.maxPolarAngle = Math.PI / 1.8
  controls.minPolarAngle = Math.PI / 8

  loadModel()
  animate()
  window.addEventListener('resize', onWindowResize)
}

// Function to load HDRI
const loadHDRI = (url) => {
  const rgbeLoader = new RGBELoader()
  rgbeLoader.load(
    url,
    (texture) => {
      if (scene.environment) {
        scene.environment.dispose()
      }
      texture.mapping = THREE.EquirectangularReflectionMapping
      scene.environment = texture
      Object.keys(materials).forEach((partName) => {
        if (materials[partName]) {
          materials[partName].envMap = scene.environment
          materials[partName].needsUpdate = true
        }
      })
      renderer.render(scene, camera)
    },
    undefined,
    (error) => console.error('Lỗi khi tải HDR environment map:', error)
  )
}

// Function to update HDRI when selection changes
const updateHDRI = () => {
  const selectedHDRI = HDRIs[selectedHDRIIndex.value]
  loadHDRI(selectedHDRI.url)
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

const onImageSelected = (event) => {
  const file = event.target.files[0]
  if (file) {
    selectedImage.value = file
    selectedImageName.value = file.name.length > 20 ? file.name.substring(0, 17) + '...' : file.name

    const reader = new FileReader()
    reader.onload = (e) => {
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

      partsToUpdate.forEach((part) => {
        if (materials[part]) {
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
        }
      })

      renderer.render(scene, camera)
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
      previewImageUrl.value = ''
    }
    selectedImage.value = null
    selectedImageName.value = ''
  }

  renderer.render(scene, camera)
}

const handleComponentChange = () => {
  console.log('Đã chọn component:', components[selectedComponentIndex.value].name)
}

const handleCustomColorChange = () => {
  selectedColor.value = customColorValue.value
}

const applyCustomColor = () => {
  const selectedPart = components[selectedComponentIndex.value].value
  const partsToUpdate = selectedPart in partGroups ? partGroups[selectedPart] : [selectedPart]

  partsToUpdate.forEach((part) => {
    if (materials[part]) {
      materials[part].color.set(customColorValue.value)
      materials[part].needsUpdate = true
      partColors[part] = customColorValue.value

      if (partTextures[part] && customTextures[part]) {
        materials[part].map = null
        materials[part].needsUpdate = true
        partTextures[part] = null
        delete customTextures[part]
      }
    }
  })

  renderer.render(scene, camera)
}

const toggleCanvasSize = () => {
  isCanvasExpanded.value = !isCanvasExpanded.value
  setTimeout(() => {
    onWindowResize()
  }, 300)
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

onMounted(() => {
  initThree()

  const urlParams = new URLSearchParams(window.location.search)
  const isEditing = urlParams.get('edit') === 'true'
  const editId = urlParams.get('id')

  if (isEditing && editId) {
    const editingDesignJson = localStorage.getItem('editingDesign')
    if (editingDesignJson) {
      try {
        const editingDesign = JSON.parse(editingDesignJson)

        if (editingDesign.id.toString() === editId) {
          console.log('Đang tải lại thiết kế...', editingDesign)

          if (editingDesign.designData && editingDesign.designData.customText) {
            customText.value = editingDesign.designData.customText
          }

          if (editingDesign.designData && editingDesign.designData.textureParams) {
            Object.assign(textureParams, editingDesign.designData.textureParams)
          }

          if (editingDesign.designData && editingDesign.designData.selectedHDRI) {
            const hdriIndex = HDRIs.findIndex(hdri => hdri.name === editingDesign.designData.selectedHDRI)
            if (hdriIndex !== -1) {
              selectedHDRIIndex.value = hdriIndex
              updateHDRI()
            }
          }

          const applyEditingDesign = () => {
            if (!model) {
              setTimeout(applyEditingDesign, 500)
              return
            }

            if (editingDesign.designData && editingDesign.designData.colors) {
              for (const partName in editingDesign.designData.colors) {
                const color = editingDesign.designData.colors[partName]
                if (materials[partName]) {
                  materials[partName].color.set(color)
                  materials[partName].needsUpdate = true
                  partColors[partName] = color
                }
              }
            }

            if (editingDesign.designData && editingDesign.designData.textures) {
              for (const partName in editingDesign.designData.textures) {
                const textureInfo = editingDesign.designData.textures[partName]
                if (materials[partName]) {
                  if (textureInfo.type === 'text' && customText.value) {
                    applyTextToMesh()
                  } else if (textureInfo.type === 'image' && editingDesign.designData.imagesData && editingDesign.designData.imagesData[partName]) {
                    const imageData = editingDesign.designData.imagesData[partName]
                    const textureLoader = new THREE.TextureLoader()
                    previewImageUrl.value = imageData
                    textureLoader.load(
                      imageData,
                      (texture) => {
                        texture.anisotropy = renderer.capabilities.getMaxAnisotropy()
                        texture.wrapS = texture.wrapT = THREE.RepeatWrapping
                        texture.flipY = false
                        texture.encoding = THREE.sRGBEncoding
                        texture.repeat.set(textureParams.repeatX * textureParams.scale, textureParams.repeatY * textureParams.scale)
                        texture.offset.set(textureParams.offsetX, textureParams.offsetY)
                        texture.rotation = textureParams.rotation
                        texture.needsUpdate = true

                        customTextures[partName] = {
                          originalMap: materials[partName].map,
                          originalColor: materials[partName].color.clone(),
                          texture,
                          imageData: imageData
                        }
                        partTextures[partName] = texture
                        materials[partName].map = texture
                        materials[partName].color.set(new THREE.Color(textureParams.brightness, textureParams.brightness, textureParams.brightness))
                        materials[partName].transparent = true
                        materials[partName].needsUpdate = true

                        renderer.render(scene, camera)
                        console.log(`Đã khôi phục texture ảnh cho phần ${partName}`)
                      },
                      undefined,
                      (error) => {
                        console.error(`Lỗi khi tải texture ảnh cho phần ${partName}:`, error)
                      }
                    )
                  }
                }
              }
            }

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
  if (scene) {
    if (scene.environment) scene.environment.dispose()
    scene.clear()
  }
  if (previewImageUrl.value) URL.revokeObjectURL(previewImageUrl.value)
})
</script>