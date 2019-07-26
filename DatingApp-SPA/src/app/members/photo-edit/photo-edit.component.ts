import { Component, OnInit, Input } from '@angular/core';
import { Photo } from 'src/app/_models/Photo';
import { FileUploader } from 'ng2-file-upload';
import { AuthService } from 'src/app/_services/auth.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-edit',
  templateUrl: './photo-edit.component.html',
  styleUrls: ['./photo-edit.component.css']
})
export class PhotoEditComponent implements OnInit {
  @Input() photos: Photo[];

  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.baseUrl;

  constructor(private authService: AuthService) {}

  ngOnInit() {
    this.initialUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initialUploader() {
    this.uploader = new FileUploader({
      url: `${this.baseUrl}users/${
        this.authService.decodedToken.nameid
      }/photos`,
      authToken: `Bearer ${localStorage.getItem('token')}`,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingAll = file => {
      file.withCredentials = false;
    };
  }
}
