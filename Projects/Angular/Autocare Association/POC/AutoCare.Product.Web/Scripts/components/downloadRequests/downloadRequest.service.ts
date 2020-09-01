
import {Injectable} from '@angular/core';
import {IDownloadRequest} from './downloadRequest.model';
import {ConstantsWarehouse} from '../constants-warehouse';
//import {HttpHelper} from '../httphelper';


@Injectable()
export class DownloadRequestService {

   //constructor(private _httpHelper: HttpHelper) { }

    getDownloadRequests() {

        let downloadRequests: IDownloadRequest[] = [];
        downloadRequests.push({
            id: 1767,
            fileType: "SQL",
            content: "VCDB, all",
            date: "06-12-2016 8.32 am PS",
            status: "Request"
        });
        downloadRequests.push({
            id: 428,
            fileType: "Comma/ASCII",
            content: "Powersport",
            date: "06-12-2016 10.44 am PST",
            status: "Download"
        });
        downloadRequests.push({
            id: 3498,
            fileType: "Access",
            content: "VCDB,ait duty",
            date: "06-12-2016 10.41 am PST",
            status: "Download"
        });

        return downloadRequests;

       // return this._httpHelper.get<IDownloadRequest[]>(ConstantsWarehouse.api.downloadRequest);
    }

}