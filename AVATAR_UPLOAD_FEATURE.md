# Avatar Upload Feature Documentation

## Tổng quan
Tính năng upload avatar cho phép người dùng tải lên hình ảnh đại diện và tự động cập nhật thông tin profile của user trong một lần gọi API.

## API Endpoints

### 1. Upload Avatar và Cập nhật Profile (Khuyến nghị)
**POST** `/api/users/upload-avatar`

Upload avatar và tự động cập nhật avatar của user profile.

#### Headers
```
Authorization: Bearer {JWT_TOKEN}
Content-Type: multipart/form-data
```

#### Parameters
- `file` (IFormFile): File hình ảnh cần upload

#### Response
```json
{
  "message": "Avatar đã được cập nhật thành công",
  "data": {
    "id": "12345678-1234-1234-1234-123456789012",
    "email": "user@example.com",
    "fullName": "Nguyen Van Linh",
    "avatar": "https://res.cloudinary.com/dmmcxdpmp/image/upload/v1234567890/avatars/avatar_12345.jpg",
    "gender": 1,
    "dateOfBirth": "1990-01-01T00:00:00Z",
    "phoneNumber": "+84123456789",
    "status": 1,
    "currency": 1,
    "lastChangePassword": "2024-01-01T00:00:00Z",
    "createdAt": "2024-01-01T00:00:00Z",
    "lastModifiedAt": "2024-01-01T12:00:00Z",
    "currentBalance": 1000000,
    "roles": ["User"]
  }
}
```

### 2. Upload Avatar File Only (Không cập nhật profile)
**POST** `/api/file/upload-avatar`

Chỉ upload file avatar mà không cập nhật user profile.

#### Headers
```
Authorization: Bearer {JWT_TOKEN}
Content-Type: multipart/form-data
```

#### Parameters
- `file` (IFormFile): File hình ảnh cần upload

#### Response
```json
{
  "data": {
    "url": "https://res.cloudinary.com/dmmcxdpmp/image/upload/v1234567890/avatars/avatar_12345.jpg",
    "fileName": "avatar.jpg",
    "size": 1024000,
    "contentType": "image/jpeg",
    "uploadedAt": "2024-01-01T12:00:00Z"
  },
  "message": "Avatar file uploaded successfully. Use /api/users/upload-avatar to upload and update user profile automatically."
}
```

## Validation Rules

### File Type
- Chỉ chấp nhận file hình ảnh: JPEG, JPG, PNG, GIF, WebP
- Content-Type được kiểm tra: `image/jpeg`, `image/jpg`, `image/png`, `image/gif`, `image/webp`

### File Size
- Tối đa 2MB cho avatar
- File vượt quá 2MB sẽ bị từ chối

### Image Processing
- Avatar sẽ được tự động resize về 400x400 pixels
- Crop mode: "fill" (cắt và scale để fit kích thước)
- Quality: auto optimization
- Format: auto optimization

## Technical Implementation

### Architecture
```
UserController.UploadAvatar()
    ↓
UploadAvatarCommand
    ↓
UploadAvatarCommandHandler
    ├── File Validation
    ├── Upload to Cloudinary
    ├── Delete Old Avatar (if exists)
    ├── Update User.Avatar
    └── Return Updated Profile
```

### Database Changes
- Trường `Avatar` trong bảng `Users` sẽ được cập nhật với URL mới
- Trường `UpdatedAt` sẽ được cập nhật thời gian hiện tại

### File Storage
- Files được lưu trên Cloudinary
- Folder: `avatars`
- Naming convention: `avatar_{userId}_{guid}.{extension}`
- Old avatar sẽ được tự động xóa (nếu có)

## Error Handling

### Common Errors
1. **No file provided**: `ValidationException("No file provided for avatar upload")`
2. **Invalid file type**: `ValidationException("Only image files (JPEG, PNG, GIF, WebP) are allowed for avatar")`
3. **File too large**: `ValidationException("Avatar file size must not exceed 2MB")`
4. **User not found**: `NotFoundException("User with ID {userId} not found")`
5. **Upload failed**: `ConflictException("Failed to upload avatar: {error}")`

### Error Response Format
```json
{
  "message": "Error message",
  "errors": ["Detailed error information"]
}
```

## Testing

### Test Cases
1. **Successful upload**: Valid image file under 2MB
2. **Invalid file type**: Upload PDF or text file
3. **File too large**: Upload image over 2MB
4. **No file**: Send request without file
5. **Invalid token**: Send request without authorization
6. **Replace existing avatar**: Upload new avatar when user already has one

### Test Files
- Sử dụng file `AvatarUpload.http` để test các endpoints
- Cần thay thế `{{token}}` bằng JWT token thực tế
- Cần có file hình ảnh test trong thư mục project

## Security Considerations

### File Validation
- Kiểm tra Content-Type của file
- Kiểm tra extension của file
- Giới hạn kích thước file
- Chỉ user đăng nhập mới được upload

### Access Control
- Chỉ user có thể upload avatar cho chính mình
- JWT token được validate trước khi process
- User ID được extract từ JWT token, không từ request

### Storage Security
- Files được lưu trên Cloudinary với unique filename
- Old avatar được xóa tự động để tránh lưu trữ không cần thiết
- URL được generated an toàn từ Cloudinary

## Performance Optimization

### Image Processing
- Tự động resize về kích thước chuẩn (400x400)
- Auto quality optimization
- Auto format optimization (WebP khi supported)
- CDN delivery từ Cloudinary

### Async Operations
- File upload và database update được thực hiện async
- Old avatar deletion không block main flow
- Error handling cho từng operation

## Monitoring và Logging

### Logs
- Upload start/success/failure
- File information (name, size, type)
- User ID và timestamp
- Old avatar deletion attempts

### Metrics
- Upload success rate
- File size distribution
- Popular image formats
- Error rate by type

## Usage Examples

### Frontend Integration
```javascript
// React/JavaScript example
const uploadAvatar = async (file) => {
  const formData = new FormData();
  formData.append('file', file);
  
  const response = await fetch('/api/users/upload-avatar', {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${token}`
    },
    body: formData
  });
  
  const result = await response.json();
  return result;
};
```

### cURL Example
```bash
curl -X POST "https://localhost:7017/api/users/upload-avatar" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -F "file=@avatar.jpg"
```

## Migration Notes

### Existing Users
- Users không có avatar sẽ có trường `Avatar` = null
- Upload avatar lần đầu sẽ set giá trị cho trường này
- Upload avatar mới sẽ replace avatar cũ

### Backward Compatibility
- API cũ `/api/file/upload-avatar` vẫn hoạt động
- Manual profile update với avatar URL vẫn supported
- Không breaking changes cho existing endpoints

## Future Enhancements

### Potential Features
1. **Multiple avatar sizes**: Generate thumbnail, medium, large versions
2. **Avatar validation**: Face detection, content moderation
3. **Avatar history**: Keep track of previous avatars
4. **Bulk operations**: Upload multiple images at once
5. **Integration**: Social media avatar import
6. **Analytics**: Avatar usage statistics

### Configuration Options
1. **File size limits**: Configurable per user role
2. **Supported formats**: Admin configurable
3. **Storage options**: Local storage fallback
4. **Processing options**: Custom resize dimensions

---

## Changelog

### Version 1.0.0 (Current)
- ✅ Basic avatar upload and profile update
- ✅ File validation (type, size)
- ✅ Cloudinary integration
- ✅ Automatic old avatar cleanup
- ✅ Comprehensive error handling
- ✅ JWT authentication
- ✅ Image processing (resize, optimization)

### Planned Updates
- [ ] Avatar history tracking
- [ ] Multiple size generation
- [ ] Content moderation
- [ ] Social media integration 