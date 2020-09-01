export class AcFile {
    file: File;
    fileName: string;
    fileSize: number = 0;
    formattedSize: string;
    contentType: string;
    fileExtension: string;
    containerName: string;
    directoryPath: string;
    chunksIdList: string;
    fileStatus: number = fileStatusEnum.pending;
    percentage: number = 0;
    fileUri: string;
    progress: any = 0 + "% 0";
    attachedBy: string;
    canDelete: boolean = false;
    attachmentId: number;
    tempContainerName: string;
}

export let fileStatusEnum = {
    pending: 0,
    uploaded: 1,
    failed: 2,
    deleted: 3
}