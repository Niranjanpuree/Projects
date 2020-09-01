import { Injectable }           from "@angular/core";
import { IUser }          from "./user.model";
import { SharedService } from '../shared/shared.service';

@Injectable()
export class UserService {
    
    constructor(private sharedService: SharedService) {
        
    }

    getSubmittedBy() {
        let userList: IUser[] = [];

        userList = this.pushDefaultUsers();
        return userList;
    }

    getUsersForAssignment() {
        let userList: IUser[]= [];
        let token = this.sharedService.getTokenModel();

        userList = this.pushDefaultUsers();
        return userList.filter(x=> (x.id !== token.customer_id && x.roleId !== "3"));
    }

    getAllUserForAssignment() {
        let userList: IUser[] = [];

        userList = this.pushDefaultUsers();
        return userList;
    }

    private pushDefaultUsers() {
        let userList: IUser[] = [];
        userList.push({
            id: "admin@autocare.com",
            user: "Admin",
            roleId: "1",
            isSelected: false
        });

        userList.push({
            id: "researcher@autocare.com",
            user: "Researcher 1",
            roleId: "2",
            isSelected: false
        });

        userList.push({
            id: "researcher2@autocare.com",
            user: "Researcher 2",
            roleId: "2",
            isSelected: false
        });

        userList.push({
            id: "user@autocare.com",
            user: "User",
            roleId: "3",
            isSelected: false
        });

        userList.push({
            id: "user2@autocare.com",
            user: "User 2",
            roleId: "3",
            isSelected: false
        });

        return userList;
    }
}