export function validateTitle(title:string) {
    if (!title.length) {
        return {
            status: true,
            value: 'Distribution Title field is required'
        }
    }
    return {
        status: false,
        value: ''
    }
}