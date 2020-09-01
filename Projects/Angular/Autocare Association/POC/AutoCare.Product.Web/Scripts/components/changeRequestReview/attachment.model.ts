
export interface IAttachment {
    attachmentId: number;
    fileName: string;
    filePath: string;
    fileExtension: string;
    fileSize: number;
    contentType: string;
    attachedBy: string;
    createdDatetime: Date;
    //canDelete: boolean;
}

//import { fileStatusEnum } from "../../lib/aclibs/ac-fileuploader/ac-fileuploader.model";
//export class AttachmentInputModel {
//    attachmentId: number = 0;
//    fileName: string = '';
//    fileSize: number = 0;
//    contentType: string = '';
//    fileExtension: string = '';
//    containerName: string = '';
//    chunksIdList: string = '';
//    attachedBy: string = '';
//    fileStatus: number = fileStatusEnum.pending;
//}